using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.IO.Pipelines;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.Connections;
using NexusTomorrow.Gateway.Tests.Sts;
using NexusTomorrow.Network.Auth.Message.Model;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners;
using NexusTomorrow.Testing;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Tests.Connections;

public sealed class ClientConnectionTests : IAsyncLifetime
{
    private static readonly TimeSpan s_wait = TimeSpan.FromSeconds(10);
    private static readonly MessageCatalog s_gate = MessageCatalog.Build([(WireMessages.Layouts, WireMessages.CreateClientToServer, WireMessages.CreateServerToClient)]);

    private readonly PipePair _pipes = new();
    private readonly CancellationTokenSource _stop = new();
    private readonly FakeTimeProvider _time = new();
    private RecordingConnection _connection;
    private Task _run = Task.CompletedTask;

    public ClientConnectionTests()
    {
        _connection = new RecordingConnection(_time, Small(4096), OpcodeTable.Pregame, MessageCatalog.Default);
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
    public async Task AMessageChangedAfterSendGoesOutAsItWasWhenSent()
    {
        // Both are queued before the writer runs, so a queue that held the message object would write the second value twice.
        var message = new LoginAccepted { DroppedForLag = 1 };
        _connection.Send(message);
        message.DroppedForLag = 2;
        _connection.Send(message);
        Start();

        Assert.Equal(1u, (await Client.ExpectAsync<LoginAccepted>()).DroppedForLag);
        Assert.Equal(2u, (await Client.ExpectAsync<LoginAccepted>()).DroppedForLag);
    }

    [Fact]
    public async Task ASendAfterTheWriterStoppedOnItsOwnDoesNotRecordAFullQueueAsTheEndReason()
    {
        var log = new RecordingLogger<ClientConnectionTests>();
        _connection = new RecordingConnection(_time, Small(4096), OpcodeTable.Pregame, MessageCatalog.Default, log);
        using var handling = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        _connection.OnMessage = _ =>
        {
            handling.Set();
            release.Wait(s_wait);
            return true;
        };
        Start();
        await Client.SendAsync(new CharacterListRequest());
        Assert.True(handling.Wait(s_wait, TestContext.Current.CancellationToken));

        // The read loop is held in the handler, so the connection has not yet recorded why it is ending when the writer stops: the
        // client's reader is gone, so the flush of this message reports the pipe completed.
        await _pipes.Client.Input.CompleteAsync();
        _connection.Send(new LoginAccepted());
        await Eventually.TrueAsync(() => _connection.LifetimeCancelled);

        _connection.Send(new LoginAccepted());
        release.Set();

        await _run.WaitAsync(s_wait, TestContext.Current.CancellationToken);
        Assert.DoesNotContain(log.Entries, entry => entry.Contains("the outbound queue is full", StringComparison.Ordinal));
        Assert.Contains(log.Entries, entry => entry.Contains("ended: closed", StringComparison.Ordinal));
    }

    [Fact]
    public async Task ASendPastACapacityQueueWhoseWriterIsLiveEndsTheConnectionAsFull()
    {
        var log = new RecordingLogger<ClientConnectionTests>();
        _connection = new RecordingConnection(_time, Small(2), OpcodeTable.Pregame, MessageCatalog.Default, log);

        // The writer has not started, so two items fill the queue and the third is refused.
        _connection.Send(new LoginAccepted());
        _connection.Send(new LoginAccepted());
        _connection.Send(new LoginAccepted());
        Start();

        await _run.WaitAsync(s_wait, TestContext.Current.CancellationToken);
        Assert.Contains(log.Entries, entry => entry.Contains("ended: the outbound queue is full", StringComparison.Ordinal));
    }

    [Fact]
    public async Task SendingAReusedMessageAllocatesNothingOnTheCallersThread()
    {
        Start();
        await Eventually.TrueAsync(() => _connection.Started);
        var message = new LoginAccepted();

        // The warm-up has two jobs: the queue's ring buffer grows to the deepest backlog it has held and then stays, so 200 backs it up
        // past anything measured; and the returned buffers stock ArrayPool.Shared, so the measured Sends find one to rent.
        for (uint i = 0; i < 200; i++)
        {
            message.DroppedForLag = i;
            _connection.Send(message);
        }

        for (int i = 0; i < 200; i++)
            await Client.ExpectAsync<LoginAccepted>();

        // Four messages at a time, read back before the next four, so the backlog stays within what ArrayPool.Shared caches per core;
        // only the Send calls are measured.
        long allocated = 0;
        for (uint batch = 0; batch < 25; batch++)
        {
            for (uint i = 0; i < 4; i++)
            {
                message.DroppedForLag = (batch * 4) + i;
                long before = GC.GetAllocatedBytesForCurrentThread();
                _connection.Send(message);
                allocated += GC.GetAllocatedBytesForCurrentThread() - before;
            }

            for (uint i = 0; i < 4; i++)
                Assert.Equal((batch * 4) + i, (await Client.ExpectAsync<LoginAccepted>()).DroppedForLag);
        }

        Assert.Equal(0L, allocated);
    }

    [Fact]
    public async Task ASharedSendWritesTheSameBytesAsASend()
    {
        Start();
        await Eventually.TrueAsync(() => _connection.Started);
        var serializer = new GatedSerializer(MessageCatalog.Default);

        _connection.Send(new LoginAccepted { DroppedForLag = 7 });
        SerializedMessage shared = serializer.Serialize(new LoginAccepted { DroppedForLag = 7 });
        _connection.Send(shared);
        shared.Release();

        Received sent = await Client.ReceiveAsync();
        Received sharedSent = await Client.ReceiveAsync();
        Assert.Equal(sent.Opcode, sharedSent.Opcode);
        Assert.Equal(sent.Body, sharedSent.Body);
        Assert.Equal(7u, ((LoginAccepted)sharedSent.Message!).DroppedForLag);
    }

    [Fact]
    public async Task TheBodyGoesBackOnceEveryWriterIsDone()
    {
        Start();
        await Eventually.TrueAsync(() => _connection.Started);
        var serializer = new GatedSerializer(MessageCatalog.Default);
        var first = new FakeClientSession();
        var second = new FakeClientSession();

        SerializedMessage shared = serializer.Serialize(new LoginAccepted { DroppedForLag = 1 });
        _connection.Send(shared);
        first.Send(shared);
        second.Send(shared);
        shared.Release();
        await Client.ExpectAsync<LoginAccepted>();

        await Eventually.TrueAsync(() => shared.References == 0);
        Assert.Throws<InvalidOperationException>(() => shared.Release());

        // Back in its pool exactly once: the next two serializations take it once and a fresh one besides.
        SerializedMessage reused = serializer.Serialize(new LoginAccepted());
        SerializedMessage other = serializer.Serialize(new LoginAccepted());
        Assert.True(ReferenceEquals(reused, shared) ^ ReferenceEquals(other, shared));
        Assert.False(ReferenceEquals(reused, other));
        reused.Release();
        other.Release();
    }

    [Fact]
    public async Task ARefusedEnqueueReleasesItsReference()
    {
        var log = new RecordingLogger<ClientConnectionTests>();
        _connection = new RecordingConnection(_time, Small(2), OpcodeTable.Pregame, MessageCatalog.Default, log);
        var serializer = new GatedSerializer(MessageCatalog.Default);

        // The writer has not started, so two items fill the queue and the shared one is refused.
        _connection.Send(new LoginAccepted());
        _connection.Send(new LoginAccepted());
        SerializedMessage shared = serializer.Serialize(new LoginAccepted());
        _connection.Send(shared);

        Assert.Equal(1, shared.References);
        shared.Release();
        Start();
        await _run.WaitAsync(s_wait, TestContext.Current.CancellationToken);
        Assert.Contains(log.Entries, entry => entry.Contains("ended: the outbound queue is full", StringComparison.Ordinal));
    }

    [Fact]
    public async Task SerializingAndSendingAllocatesNothingOnceWarm()
    {
        Start();
        await Eventually.TrueAsync(() => _connection.Started);
        var serializer = new GatedSerializer(MessageCatalog.Default);
        var message = new LoginAccepted();

        // The warm-up grows the queue's ring and stocks both pools, so the measured rounds find a body and a holder to rent.
        for (uint i = 0; i < 200; i++)
        {
            message.DroppedForLag = i;
            SerializedMessage shared = serializer.Serialize(message);
            _connection.Send(shared);
            shared.Release();
        }

        for (int i = 0; i < 200; i++)
            await Client.ExpectAsync<LoginAccepted>();

        long allocated = 0;
        for (uint batch = 0; batch < 25; batch++)
        {
            for (uint i = 0; i < 4; i++)
            {
                message.DroppedForLag = (batch * 4) + i;
                long before = GC.GetAllocatedBytesForCurrentThread();
                SerializedMessage shared = serializer.Serialize(message);
                _connection.Send(shared);
                shared.Release();
                allocated += GC.GetAllocatedBytesForCurrentThread() - before;
            }

            for (uint i = 0; i < 4; i++)
                Assert.Equal((batch * 4) + i, (await Client.ExpectAsync<LoginAccepted>()).DroppedForLag);
        }

        Assert.Equal(0L, allocated);
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
    public async Task AMessagePastItsRateIsDroppedUnreadAndTheSessionGoesOn()
    {
        FrozenDictionary<ushort, OpcodeRow> table = new OpcodeRow[]
        {
            new(0x07E0, MessageOwner.Realm, MessagePriority.Normal, 1, OverLimitAction.Drop),
            new(0x0352, MessageOwner.Realm, MessagePriority.Normal, 1, OverLimitAction.Drop),
        }.ToFrozenDictionary(row => row.Opcode);
        Use(Small(4096), table);
        var handled = new ConcurrentQueue<Type>();
        _connection.OnMessage = message =>
        {
            handled.Enqueue(message.GetType());
            return true;
        };
        Start();

        // The clock never moves, so the first request spends 0x07E0's one token. CharacterListRequest declares no fields, so the
        // second request's byte would be malformed if it were read.
        await Client.SendAsync(new CharacterListRequest());
        await Client.SendUncheckedAsync(0x07E0, new RawBody([0xFF]));
        await Client.SendAsync(new CharacterDelete { CharacterId = 5 });

        await Eventually.TrueAsync(() => handled.Count == 2 || _run.IsCompleted);
        Assert.Equal(0, _connection.Closed);
        Type[] expected = [typeof(CharacterListRequest), typeof(CharacterDelete)];
        Assert.Equal(expected, handled);
    }

    [Fact]
    public void SendingAMessageTheCatalogDoesNotHoldThrows()
    {
        Assert.Throws<InvalidOperationException>(() => _connection.Send(new UnverifiedForTest()));
    }

    [Fact]
    public void SendingAPurposeMessageWithAnUnverifiedMemberSetThrowsNamingTheMember()
    {
        Use(Small(4096), catalog: s_gate);

        InvalidOperationException thrown = Assert.Throws<InvalidOperationException>(() => _connection.Send(new GatePurposeMessage { Unread = 1 }));

        Assert.Contains("GatePurposeMessage.Unread", thrown.Message);
    }

    [Fact]
    public async Task APurposeMessageWithOnlyVerifiedMembersSetReachesTheClient()
    {
        Use(Small(4096), catalog: s_gate);
        Start();
        await Eventually.TrueAsync(() => _connection.Started);

        _connection.Send(new GatePurposeMessage { Known = 1 });

        ReadResult read = await _pipes.Client.Input.ReadAsync(TestContext.Current.CancellationToken);
        Assert.True(read.Buffer.Length > 0);
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

    private void Use(GatewayOptions options, FrozenDictionary<ushort, OpcodeRow>? table = null, MessageCatalog? catalog = null) =>
        _connection = new RecordingConnection(_time, options, table ?? OpcodeTable.Pregame, catalog ?? MessageCatalog.Default);

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

        public RecordingConnection(TimeProvider time, GatewayOptions options, FrozenDictionary<ushort, OpcodeRow> table, MessageCatalog catalog, ILogger? logger = null)
            : base(0x0076, catalog, new InboundDecoder(catalog, table, new HashSet<ushort>()),
                Options.Create(options), time, new GatewayMetrics(), logger ?? NullLogger.Instance)
        {
        }

        public Action<RecordingConnection> OnStartup { get; set; } = _ => { };

        public MessageHandler OnMessage { get; set; } = _ => true;

        public bool LifetimeCancelled => Lifetime.IsCancellationRequested;

        public bool Started => Volatile.Read(ref _started) == 1;

        public int Closed => Volatile.Read(ref _closed);

        public void SwitchKeyTo(PacketCipher cipher) => SwitchKey(cipher);

        // Every row is handled in every state, so only the decoder and the rate gate refuse a message here.
        protected override bool Admits(OpcodeRow row) => true;

        protected override ValueTask<bool> HandleAsync(IWireMessage message, OpcodeRow row) => ValueTask.FromResult(OnMessage(message));

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

        public string? FindUnverifiedValue() => null;

        public bool IsDefault() => true;

        public void Write(ref BitWriter writer)
        {
        }

        public void Read(ref BitReader reader)
        {
        }
    }
}
