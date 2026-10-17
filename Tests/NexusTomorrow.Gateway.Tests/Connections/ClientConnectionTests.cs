using System.Collections.Frozen;
using System.IO.Pipelines;
using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.Connections;
using NexusTomorrow.Network.Auth.Message.Model;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Testing;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Tests.Connections;

public sealed class ClientConnectionTests : IAsyncLifetime
{
    private static readonly TimeSpan s_wait = TimeSpan.FromSeconds(10);

    private readonly PipePair _pipes = new();
    private readonly CancellationTokenSource _stop = new();
    private readonly FakeTimeProvider _time = new();
    private RecordingConnection _connection;
    private Task _run = Task.CompletedTask;

    public ClientConnectionTests()
    {
        _connection = new RecordingConnection(_time, Small(4096), OpcodeTable.Pregame);
        Client = new GameTestClient(_pipes.Client);
    }

    private GameTestClient Client { get; }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        await _stop.CancelAsync();
        await _run.WaitAsync(s_wait);
    }

    [Fact]
    public async Task ADisconnectFromAnotherThreadSendsWhatIsQueuedThenEndsTheConnectionOnce()
    {
        Start();
        await Eventually.TrueAsync(() => _connection.Started);

        _connection.Send(new LoginAccepted { DroppedForLag = 3 });
        _connection.Disconnect("test");

        Assert.Equal(3u, (await Client.ExpectAsync<LoginAccepted>()).DroppedForLag);
        Assert.True(await Client.IsClosedAsync(s_wait));
        await _run.WaitAsync(s_wait, TestContext.Current.CancellationToken);
        Assert.Equal(1, _connection.Closed);
    }

    [Fact]
    public async Task TheClientClosingItsEndEndsTheConnectionOnce()
    {
        Start();
        await Eventually.TrueAsync(() => _connection.Started);

        await _pipes.Client.Output.CompleteAsync();

        await _run.WaitAsync(s_wait, TestContext.Current.CancellationToken);
        Assert.Equal(1, _connection.Closed);
    }

    [Fact]
    public async Task AHandlerThatThrowsEndsTheConnectionOnceAndCompletesTheRun()
    {
        _connection.OnMessage = _ => throw new InvalidOperationException("a handler bug");
        Start();

        await Client.SendAsync(new CharacterListRequest());

        await _run.WaitAsync(s_wait, TestContext.Current.CancellationToken);
        Assert.Equal(1, _connection.Closed);
        Assert.True(await Client.IsClosedAsync(s_wait));
    }

    [Fact]
    public async Task AKeySwitchTakesEffectBetweenTheMessagesQueuedBeforeAndAfterIt()
    {
        var next = new PacketCipher(0x1122334455667788UL);
        _connection.OnStartup = connection =>
        {
            connection.Send(new LoginAccepted { DroppedForLag = 1 });
            connection.SwitchKeyTo(next);
            connection.Send(new LoginAccepted { DroppedForLag = 2 });
        };
        Start();

        Assert.Equal(1u, (await Client.ExpectAsync<LoginAccepted>()).DroppedForLag);
        Client.Cipher = next;
        Assert.Equal(2u, (await Client.ExpectAsync<LoginAccepted>()).DroppedForLag);
    }

    [Fact]
    public async Task AKeySwitchAppliesToTheNextFrameTheClientSends()
    {
        var next = new PacketCipher(0x1122334455667788UL);
        var seen = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
        int handled = 0;
        _connection.OnMessage = message =>
        {
            if (Interlocked.Increment(ref handled) == 1)
                _connection.SwitchKeyTo(next);
            else
                seen.TrySetResult(message);
            return true;
        };
        Start();

        await Client.SendAsync(new CharacterListRequest());
        Client.Cipher = next;
        await Client.SendAsync(new CharacterListRequest());

        Assert.IsType<CharacterListRequest>(await seen.Task.WaitAsync(s_wait, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task APeerThatPingsAndNeverReadsIsDisconnectedOnceTheQueueIsFull()
    {
        Use(Small(16));
        Start();
        await Eventually.TrueAsync(() => _connection.Started);

        await Client.SendRawAsync(Pings(PingsThatFillThePipe));

        await EndedAfterTheGraceAsync();
        Assert.Equal(1, _connection.Closed);
    }

    [Fact]
    public async Task ADisconnectEndsAConnectionWhoseWriterIsBlockedOnceTheGraceHasPassed()
    {
        Use(Small(100_000));
        var sentinel = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        _connection.OnMessage = _ =>
        {
            sentinel.TrySetResult();
            return true;
        };
        Start();
        await Eventually.TrueAsync(() => _connection.Started);

        // The sentinel is handled after every ping before it was answered, so the answers outnumber what the pipe holds and the
        // writer is blocked by the time it arrives.
        await Client.SendRawAsync([.. Pings(PingsThatFillThePipe), .. Client.Envelope(0x07E0, new CharacterListRequest())]);
        await sentinel.Task.WaitAsync(s_wait, TestContext.Current.CancellationToken);

        _connection.Disconnect("kicked");

        await EndedAfterTheGraceAsync();
        Assert.Equal(1, _connection.Closed);
    }

    [Fact]
    public async Task AReadLoopThatEndsOnAMalformedFrameStillClosesAStalledConnectionAfterTheGrace()
    {
        Use(Small(100_000));
        Start();
        await Eventually.TrueAsync(() => _connection.Started);

        // The pings fill the pipe so the writer blocks, then a frame over the cap ends the read loop first.
        await Client.SendRawAsync([.. Pings(PingsThatFillThePipe), 0x00, 0x00, 0x10, 0x00, 0x44, 0x02]);

        await EndedAfterTheGraceAsync();
        Assert.Equal(1, _connection.Closed);
    }

    [Fact]
    public async Task AReadLoopThatEndsOnACleanCloseStillClosesAStalledConnectionAfterTheGrace()
    {
        Use(Small(100_000));
        Start();
        await Eventually.TrueAsync(() => _connection.Started);

        await Client.SendRawAsync(Pings(PingsThatFillThePipe));
        await _pipes.Client.Output.CompleteAsync();

        await EndedAfterTheGraceAsync();
        Assert.Equal(1, _connection.Closed);
    }

    [Fact]
    public async Task TheWriterFlushesEveryBatchAndOnceMoreWhenTheQueueDrains()
    {
        Use(Small(4096));
        var message = new LoginAccepted();
        for (int i = 0; i < 200; i++)
            _connection.Send(message);
        var output = new CountingWriter(_pipes.Server.Output);

        Start(new Transport(_pipes.Server.Input, output));

        // 200 queued items flush after the 64th, 128th and 192nd, then once when the queue is empty.
        await Eventually.TrueAsync(() => output.Flushes >= 4);
        Assert.Equal(4, output.Flushes);
    }
    [Fact]
    public async Task TheConnectionRateGatesByTheTableItsDecoderWasBuiltWith()
    {
        FrozenDictionary<ushort, OpcodeRow> strict = new OpcodeRow[]
        {
            new(0x07E0, MessageOwner.Realm, MessagePriority.Normal, 1, OverLimitAction.Disconnect),
        }.ToFrozenDictionary(row => row.Opcode);
        Use(Small(4096), strict);
        int handled = 0;
        _connection.OnMessage = _ =>
        {
            Interlocked.Increment(ref handled);
            return true;
        };
        Start();

        await Client.SendAsync(new CharacterListRequest());
        await Client.SendAsync(new CharacterListRequest());

        await _run.WaitAsync(s_wait, TestContext.Current.CancellationToken);
        Assert.Equal(1, handled);
        Assert.Equal(1, _connection.Closed);
    }

    [Fact]
    public void SendingAMessageBelowFieldsThrows()
    {
        Assert.Throws<InvalidOperationException>(() => _connection.Send(new UnverifiedForTest()));
    }

    // Each ping is answered with a 7-byte frame, so this many answers exceed the 64 KiB a pipe holds before its writer waits.
    private const int PingsThatFillThePipe = 12_000;

    private static GatewayOptions Small(int capacity) => new()
    {
        UnknownAccountSecret = "",
        PreAuthTimeout = TimeSpan.FromHours(1),
        OutboundQueueCapacity = capacity,
        DisconnectGrace = TimeSpan.FromSeconds(5),
    };

    private static byte[] Pings(int count)
    {
        byte[] bytes = new byte[count * 7];
        for (int i = 0; i < count; i++)
            bytes[i * 7] = 7;
        return bytes;
    }

    private void Use(GatewayOptions options, FrozenDictionary<ushort, OpcodeRow>? table = null) =>
        _connection = new RecordingConnection(_time, options, table ?? OpcodeTable.Pregame);

    // The grace runs on the fake clock, so the clock is advanced until the run completes; the pre-auth timeout is far longer.
    private async Task EndedAfterTheGraceAsync() =>
        await Eventually.TrueAsync(
            () =>
            {
                _time.Advance(TimeSpan.FromSeconds(1));
                return _run.IsCompleted;
            });

    private void Start() => Start(_pipes.Server);

    private void Start(IDuplexPipe transport) =>
        _run = _connection.RunAsync(transport, new IPEndPoint(IPAddress.Loopback, 50001), _stop.Token);

    private sealed record Transport(PipeReader Input, PipeWriter Output) : IDuplexPipe;

    // Counts the flushes the connection's writer makes.
    private sealed class CountingWriter : PipeWriter
    {
        private readonly PipeWriter _inner;
        private int _flushes;

        public CountingWriter(PipeWriter inner)
        {
            _inner = inner;
        }

        public int Flushes => Volatile.Read(ref _flushes);

        public override void Advance(int bytes) => _inner.Advance(bytes);

        public override Memory<byte> GetMemory(int sizeHint = 0) => _inner.GetMemory(sizeHint);

        public override Span<byte> GetSpan(int sizeHint = 0) => _inner.GetSpan(sizeHint);

        public override void CancelPendingFlush() => _inner.CancelPendingFlush();

        public override void Complete(Exception? exception = null) => _inner.Complete(exception);

        public override ValueTask<FlushResult> FlushAsync(CancellationToken cancellationToken = default)
        {
            Interlocked.Increment(ref _flushes);
            return _inner.FlushAsync(cancellationToken);
        }
    }

    // A delegate, because IWireMessage declares static abstract members and so cannot be a type argument of Func.
    private delegate bool MessageHandler(IWireMessage message);

    private sealed class RecordingConnection : ClientConnection
    {
        private int _closed;
        private int _started;

        public RecordingConnection(TimeProvider time, GatewayOptions options, FrozenDictionary<ushort, OpcodeRow> table)
            : base(0x0076, MessageCatalog.Default, new InboundDecoder(MessageCatalog.Default, table, new HashSet<ushort>()),
                Options.Create(options), time, new GatewayMetrics(), NullLogger.Instance)
        {
        }

        public Action<RecordingConnection> OnStartup { get; set; } = _ => { };

        public MessageHandler OnMessage { get; set; } = _ => true;

        public bool Started => Volatile.Read(ref _started) == 1;

        public int Closed => Volatile.Read(ref _closed);

        public void SwitchKeyTo(PacketCipher cipher) => SwitchKey(cipher);

        protected override ValueTask<bool> HandleAsync(IWireMessage message) => ValueTask.FromResult(OnMessage(message));

        protected override void OnStart()
        {
            OnStartup(this);
            Volatile.Write(ref _started, 1);
        }

        protected override void OnClosed() => Interlocked.Increment(ref _closed);
    }

    // A message no declaration verifies, so the catalog must refuse to send it.
    private sealed class UnverifiedForTest : IWireMessage
    {
        public static ushort Opcode => 0xFFFF;

        public static Direction Direction => Direction.ServerToClient;

        public static WireLayout Layout => throw new NotSupportedException();

        public int MeasureBits() => 0;

        public void Write(ref BitWriter writer)
        {
        }

        public void Read(ref BitReader reader)
        {
        }
    }
}
