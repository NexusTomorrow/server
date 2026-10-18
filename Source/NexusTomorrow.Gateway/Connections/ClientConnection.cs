using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Owners;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Connections;

/// <summary>
/// A game connection: one read loop that frames, answers transport pings and decodes, and one writer that drains the session's
/// outbound queue in order and flushes once per drain. Owners reach it through <see cref="IClientSession"/> from any thread.
/// </summary>
public abstract class ClientConnection : IClientSession
{
    // Items written between flushes, so a queue that never empties still reaches the client.
    private const int FlushBatch = 64;

    private static long s_nextId;

    private readonly ushort _envelopeOpcode;
    private readonly MessageCatalog _catalog;
    private readonly InboundDecoder _decoder;
    private readonly GatewayOptions _options;
    private readonly GatewayMetrics _metrics;
    private readonly ILogger _logger;
    private readonly RateGate _rates;
    private readonly Channel<Outbound> _outbound;
    private readonly TaskCompletionSource _disconnected = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private PacketCipher _receiveCipher = new(PacketCipher.BuildKey);
    private CancellationTokenSource? _preAuth;
    private CancellationToken _lifetime;
    private string? _disconnectReason;

    /// <summary>
    /// Create a connection whose server messages travel inside <paramref name="envelopeOpcode"/>.
    /// </summary>
    protected ClientConnection(ushort envelopeOpcode, MessageCatalog catalog, InboundDecoder decoder, IOptions<GatewayOptions> options, TimeProvider time, GatewayMetrics metrics, ILogger logger)
    {
        _envelopeOpcode = envelopeOpcode;
        _catalog = catalog;
        _decoder = decoder;
        _options = options.Value;
        _metrics = metrics;
        _logger = logger;
        Time = time;
        _rates = new RateGate(time);
        _outbound = Channel.CreateBounded<Outbound>(new BoundedChannelOptions(_options.OutboundQueueCapacity) { SingleReader = true, FullMode = BoundedChannelFullMode.Wait });
    }

    /// <inheritdoc/>
    public long Id { get; } = Interlocked.Increment(ref s_nextId);

    /// <inheritdoc/>
    public IPAddress RemoteAddress { get; private set; } = IPAddress.None;

    /// <summary>
    /// The clock the connection reads.
    /// </summary>
    protected TimeProvider Time { get; }

    /// <summary>
    /// Fires when the connection is ending, whatever ended it. A handler that waits on something outside the connection waits on this
    /// token too, so it cannot outlive the connection.
    /// </summary>
    protected CancellationToken Lifetime => _lifetime;

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">The send gate refuses <paramref name="message"/> (<see cref="MessageCatalog.MaySend"/>).</exception>
    public void Send<T>(T message)
        where T : class, IWireMessage
    {
        EnsureSendable(message);
        Enqueue(Outbound.Enveloped(T.Opcode, message));
    }

    /// <summary>
    /// End the connection after the messages already queued, logging <paramref name="reason"/>. A client that has stopped reading
    /// cannot hold the connection open: after <see cref="GatewayOptions.DisconnectGrace"/> both loops are cancelled and the transport
    /// is closed.
    /// </summary>
    public void Disconnect(string reason)
    {
        Interlocked.CompareExchange(ref _disconnectReason, reason, null);
        _outbound.Writer.TryComplete();
        _disconnected.TrySetResult();
    }

    /// <summary>
    /// Serve the connection until the client closes it, it is disconnected, the pre-auth timeout passes, or
    /// <paramref name="cancellationToken"/> fires. <see cref="OnClosed"/> runs once, after both loops have ended.
    /// </summary>
    public async Task RunAsync(IDuplexPipe transport, IPEndPoint remote, CancellationToken cancellationToken)
    {
        RemoteAddress = remote.Address;
        using var preAuth = new CancellationTokenSource(_options.PreAuthTimeout, Time);
        using var writerDone = new CancellationTokenSource();
        using var forced = new CancellationTokenSource();
        using var finished = new CancellationTokenSource();
        using var lifetime = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, preAuth.Token, writerDone.Token, forced.Token);
        _preAuth = preAuth;
        _lifetime = lifetime.Token;
        Task writer = WriteAsync(transport.Output, writerDone, lifetime.Token);
        Task watchdog = ForceAfterGraceAsync(forced, finished.Token);
        try
        {
            OnStart();
            await ReadAsync(transport.Input, lifetime.Token);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            GatewayLog.ConnectionFailed(_logger, Id, exception);
        }
        finally
        {
            // However the read loop ended, the writer gets the grace to deliver what is queued; a peer that is not reading cannot hold
            // RunAsync past it.
            string reason = _disconnectReason ?? (preAuth.IsCancellationRequested ? "no login before the timeout" : "closed");
            Disconnect(reason);
            try
            {
                await writer;
                await finished.CancelAsync();
                await watchdog;
                await transport.Input.CompleteAsync();
            }
            catch (Exception exception)
            {
                GatewayLog.ConnectionFailed(_logger, Id, exception);
            }

            GatewayLog.ConnectionEnded(_logger, Id, _disconnectReason ?? reason);
            OnClosed();
        }
    }

    /// <summary>
    /// Handle one decoded message, admitted by <paramref name="row"/>; false ends the connection.
    /// </summary>
    protected abstract ValueTask<bool> HandleAsync(IWireMessage message, OpcodeRow row);

    /// <summary>
    /// Called once when the connection starts, before anything is read.
    /// </summary>
    protected virtual void OnStart()
    {
    }

    /// <summary>
    /// Called once after the connection has ended and its writer has finished.
    /// </summary>
    protected virtual void OnClosed()
    {
    }

    /// <summary>
    /// Queue <paramref name="message"/> as a bare frame, outside any envelope.
    /// </summary>
    protected void SendBare<T>(T message)
        where T : class, IWireMessage
    {
        EnsureSendable(message);
        Enqueue(Outbound.Bare(T.Opcode, message));
    }

    /// <summary>
    /// Count and log a message the connection read but does not act on; the session goes on.
    /// </summary>
    protected void Drop(ushort opcode, string reason)
    {
        _metrics.Dropped(opcode, reason);
        GatewayLog.MessageDropped(_logger, Id, opcode, reason);
    }

    /// <summary>
    /// Switch the connection to <paramref name="cipher"/>: frames read after this call are decrypted with it at once, and messages
    /// queued after this call are encrypted with it. Call it from <see cref="HandleAsync"/> or <see cref="OnStart"/>, the only places
    /// the read loop is not decoding.
    /// </summary>
    protected void SwitchKey(PacketCipher cipher)
    {
        _receiveCipher = cipher;
        Enqueue(Outbound.Key(cipher));
    }

    /// <summary>
    /// The login is complete: lift the pre-auth timeout.
    /// </summary>
    protected void MarkAuthenticated() => _preAuth?.CancelAfter(Timeout.InfiniteTimeSpan);

    // A full queue means the client is not reading; the sender is never made to wait, so the connection ends instead.
    private void Enqueue(in Outbound item)
    {
        if (!_outbound.Writer.TryWrite(item))
            Disconnect("the outbound queue is full");
    }

    private async Task ForceAfterGraceAsync(CancellationTokenSource forced, CancellationToken finished)
    {
        try
        {
            await _disconnected.Task.WaitAsync(finished);
            await Task.Delay(_options.DisconnectGrace, Time, finished);
            await forced.CancelAsync();
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void EnsureSendable(IWireMessage message)
    {
        if (!_catalog.MaySend(message, out string? refusal))
            throw new InvalidOperationException($"{refusal}, so the server may not send it.");
    }

    private async Task ReadAsync(PipeReader input, CancellationToken cancellationToken)
    {
        while (true)
        {
            ReadResult result = await input.ReadAsync(cancellationToken);
            ReadOnlySequence<byte> buffer = result.Buffer;
            try
            {
                while (true)
                {
                    FrameStatus status = FrameReader.TryRead(ref buffer, _options.MaxClientFrameBytes, out ushort opcode, out ReadOnlySequence<byte> body);
                    if (status == FrameStatus.Incomplete)
                        break;
                    if (status == FrameStatus.Malformed)
                    {
                        Disconnect("a frame shorter than its header or over the cap");
                        return;
                    }

                    // Decoding waits for the previous message's handling, so a key switched while handling it applies to this frame.
                    if (!await OnFrameAsync(opcode, body.ToArray()))
                        return;
                }
            }
            finally
            {
                input.AdvanceTo(buffer.Start, buffer.End);
            }

            if (result.IsCompleted)
                return;
        }
    }

    private async ValueTask<bool> OnFrameAsync(ushort opcode, byte[] body)
    {
        switch (opcode)
        {
            case TransportOpcodes.Ping:
                if (body.Length != 1)
                {
                    Disconnect("a ping without exactly one index byte");
                    return false;
                }

                Enqueue(Outbound.Ping(body[0]));
                return true;
            case TransportOpcodes.PingAnswer or TransportOpcodes.Ignored:
                return true;
        }

        Inbound inbound;
        try
        {
            inbound = _decoder.Decode(opcode, body, _receiveCipher);
        }
        catch (Exception exception)
        {
            // The decoder turns a codec fault into Malformed; anything else is a declaration bug, and it ends only this connection.
            GatewayLog.ConnectionFailed(_logger, Id, exception);
            Disconnect($"an unexpected failure decoding frame 0x{opcode:X4}");
            return false;
        }

        switch (inbound.Result)
        {
            case InboundResult.Dropped:
                Drop(inbound.Opcode, inbound.Reason);
                return true;
            case InboundResult.Malformed:
                if (_logger.IsEnabled(LogLevel.Debug))
                    GatewayLog.MessageMalformed(_logger, Id, inbound.Opcode);

                Disconnect($"a malformed message 0x{inbound.Opcode:X4}: {inbound.Reason}");
                return false;
        }

        OpcodeRow row = _decoder.RowFor(inbound.Opcode);
        if (!_rates.TryPass(row))
        {
            if (row.OverLimit == OverLimitAction.Disconnect)
            {
                Disconnect($"0x{inbound.Opcode:X4} over its rate limit");
                return false;
            }

            _metrics.Dropped(inbound.Opcode, "over its rate limit");
            return true;
        }

        // A message the decoder admitted is non-null by InboundResult.Message's contract.
        IWireMessage admitted = inbound.Message!;
        if (_logger.IsEnabled(LogLevel.Debug))
            GatewayLog.MessageReceived(_logger, Id, inbound.Opcode, admitted.GetType().Name);

        return await HandleAsync(admitted, row);
    }

    // Names the message type only: field values and payload bytes (RealmHandoff carries the session key) never reach a log line.
    private void LogSent(in Outbound item)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            GatewayLog.MessageSent(_logger, Id, item.Opcode, item.Message!.GetType().Name);
    }

    private async Task WriteAsync(PipeWriter output, CancellationTokenSource writerDone, CancellationToken cancellationToken)
    {
        PacketCipher sendCipher = new(PacketCipher.BuildKey);
        ChannelReader<Outbound> reader = _outbound.Reader;
        try
        {
            while (await reader.WaitToReadAsync(cancellationToken))
            {
                int batched = 0;
                while (reader.TryRead(out Outbound item))
                {
                    // Message is set by the Bare and Enveloped factories and Cipher by Key, the only ways to build those kinds.
                    switch (item.Kind)
                    {
                        case OutboundKind.Bare:
                            FrameWriter.WriteMessage(output, item.Opcode, item.Message!);
                            LogSent(item);
                            break;
                        case OutboundKind.Enveloped:
                            FrameWriter.WriteEnvelope(output, _envelopeOpcode, item.Opcode, item.Message!, sendCipher);
                            LogSent(item);
                            break;
                        case OutboundKind.Key:
                            sendCipher = item.Cipher!;
                            break;
                        case OutboundKind.PingAnswer:
                            FrameWriter.WritePing(output, TransportOpcodes.PingAnswer, item.PingIndex);
                            break;
                    }

                    if (++batched == FlushBatch)
                    {
                        batched = 0;
                        if ((await output.FlushAsync(cancellationToken)).IsCompleted)
                            return;
                    }
                }

                if ((await output.FlushAsync(cancellationToken)).IsCompleted)
                    return;
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception)
        {
            GatewayLog.ConnectionFailed(_logger, Id, exception);
        }
        finally
        {
            await output.CompleteAsync();
            await writerDone.CancelAsync();
        }
    }
}
