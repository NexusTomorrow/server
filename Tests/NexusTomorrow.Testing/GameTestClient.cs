using System.Buffers;
using System.IO.Pipelines;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message.Model;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Testing;

// One game frame the client received: the frame's opcode, and for an envelope the inner opcode and message.
public sealed record Received(ushort FrameOpcode, ushort Opcode, IWireMessage? Message, byte[] Body);

// The client side of a login or world connection: sends messages inside 0x0244 under its current cipher, receives and opens the
// server's envelopes.
public sealed class GameTestClient
{
    public const ushort ClientEnvelope = 0x0244;
    public const ushort NestedOpcode = 0x025C;
    public const ushort TaggedOpcode = 0x038C;

    private readonly IDuplexPipe _pipe;
    private byte[] _pending = [];

    public GameTestClient(IDuplexPipe pipe)
    {
        _pipe = pipe;
    }

    public PacketCipher Cipher { get; set; } = new(PacketCipher.BuildKey);

    public async Task SendAsync<T>(T message)
        where T : class, IWireMessage
    {
        await _pipe.Output.WriteAsync(Envelope(T.Opcode, message));
    }

    public async Task SendNestedAsync<T>(T message)
        where T : class, IWireMessage
    {
        var inner = new ArrayBufferWriter<byte>();
        FrameWriter.WriteMessage(inner, T.Opcode, message);
        await SendAsync(new NestedMessage { Payload = inner.WrittenSpan[4..].ToArray() });
    }

    // A world message as the client sends it: inside 0x038C, whose body is the tag in the low 5 bits of its first byte and then the
    // whole inner frame (a 32-bit length counting itself, the opcode, the body).
    public async Task SendTaggedAsync<T>(byte tag, T message)
        where T : class, IWireMessage => await SendTaggedUncheckedAsync(tag, T.Opcode, message);

    // Any opcode and body under a tag, for messages the server must refuse.
    public async Task SendTaggedUncheckedAsync(byte tag, ushort opcode, IWireStruct message)
    {
        var inner = new ArrayBufferWriter<byte>();
        FrameWriter.WriteMessage(inner, opcode, message);
        await SendUncheckedAsync(TaggedOpcode, new RawBody([(byte)(tag & 0x1F), .. inner.WrittenSpan]));
    }

    // Any opcode and body, for messages the server must refuse.
    public async Task SendUncheckedAsync(ushort opcode, IWireStruct message) =>
        await _pipe.Output.WriteAsync(Envelope(opcode, message));

    public async Task SendRawAsync(byte[] bytes) => await _pipe.Output.WriteAsync(bytes);

    public byte[] Envelope(ushort opcode, IWireStruct message)
    {
        var output = new ArrayBufferWriter<byte>();
        FrameWriter.WriteEnvelope(output, ClientEnvelope, opcode, message, Cipher);
        return output.WrittenSpan.ToArray();
    }

    public async Task<Received> ReceiveAsync(TimeSpan? timeout = null)
    {
        using var cancel = new CancellationTokenSource(timeout ?? TimeSpan.FromSeconds(10));
        while (true)
        {
            var buffer = new ReadOnlySequence<byte>(_pending);
            FrameStatus status = FrameReader.TryRead(ref buffer, int.MaxValue, out ushort opcode, out ReadOnlySequence<byte> body);
            if (status == FrameStatus.Complete)
            {
                byte[] bytes = body.ToArray();
                _pending = buffer.ToArray();
                return Open(opcode, bytes);
            }

            ReadResult result = await _pipe.Input.ReadAsync(cancel.Token);
            _pending = [.. _pending, .. result.Buffer.ToArray()];
            _pipe.Input.AdvanceTo(result.Buffer.End);
            if (result.IsCompleted && result.Buffer.IsEmpty)
                throw new EndOfStreamException("The server closed the connection.");
        }
    }

    public async Task<T> ExpectAsync<T>(TimeSpan? timeout = null)
        where T : class, IWireMessage
    {
        Received received = await ReceiveAsync(timeout);
        return received.Message as T
            ?? throw new InvalidOperationException($"Expected {typeof(T).Name}, received 0x{received.Opcode:X4} ({received.Message?.GetType().Name ?? "no message"}).");
    }

    public async Task<bool> IsClosedAsync(TimeSpan timeout)
    {
        using var cancel = new CancellationTokenSource(timeout);
        try
        {
            while (true)
            {
                ReadResult result = await _pipe.Input.ReadAsync(cancel.Token);
                _pipe.Input.AdvanceTo(result.Buffer.End);
                if (result.IsCompleted)
                    return true;
            }
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    private Received Open(ushort frameOpcode, byte[] body)
    {
        if (frameOpcode is TransportOpcodes.Ping or TransportOpcodes.PingAnswer)
            return new Received(frameOpcode, frameOpcode, null, body);

        ushort opcode = frameOpcode;
        Span<byte> inner = body;
        if (frameOpcode is 0x0076 or 0x03DC)
        {
            if (!EnvelopeReader.TryOpen(body, Cipher, out opcode, out inner))
                throw new InvalidDataException("The server's envelope length is not its own.");
        }

        IWireMessage message = ServerMessages.Create(opcode) ?? throw new InvalidDataException($"No server message 0x{opcode:X4}.");
        var reader = new BitReader(inner);
        message.Read(ref reader);
        if (reader.BitsRemaining >= 8)
            throw new InvalidDataException($"0x{opcode:X4} left {reader.BitsRemaining} bits unread.");
        return new Received(frameOpcode, opcode, message, inner.ToArray());
    }
}
