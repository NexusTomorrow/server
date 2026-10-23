#if DEBUG
using System.Collections.Concurrent;
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

namespace NexusTomorrow.Gateway.Tests.Connections;

public sealed class DebugSessionTests : IAsyncLifetime
{
    private static readonly TimeSpan s_wait = TimeSpan.FromSeconds(10);

    private readonly PipePair _pipes = new();
    private readonly CancellationTokenSource _stop = new();
    private readonly TapConnection _connection;
    private readonly GameTestClient _client;
    private Task _run = Task.CompletedTask;

    public DebugSessionTests()
    {
        var options = new GatewayOptions
        {
            UnknownAccountSecret = "",
            PreAuthTimeout = TimeSpan.FromHours(1),
            OutboundQueueCapacity = 64,
            DisconnectGrace = TimeSpan.FromSeconds(5),
        };
        _connection = new TapConnection(new FakeTimeProvider(), options);
        _client = new GameTestClient(_pipes.Client);
    }

    public ValueTask InitializeAsync()
    {
        _run = _connection.RunAsync(_pipes.Server, new IPEndPoint(IPAddress.Loopback, 50002), _stop.Token);
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _stop.CancelAsync();
        await _run.WaitAsync(s_wait);
    }

    [Fact]
    public async Task AnUncheckedSendSkipsTheGateAndReachesTheClient()
    {
        var message = new Server0015();
        Assert.Throws<InvalidOperationException>(() => _connection.Send(message));

        _connection.SendUnchecked(OpcodeOf<Server0015>(), message);

        Received received = await _client.ReceiveAsync(s_wait);
        Assert.Equal(OpcodeOf<Server0015>(), received.Opcode);
        Assert.IsType<Server0015>(received.Message);
    }

    [Fact]
    public async Task ARawBodyReachesTheClientExactlyAsGiven()
    {
        _connection.Send(new LoginAccepted { DroppedForLag = 5 });
        Received built = await _client.ReceiveAsync(s_wait);

        _connection.SendRaw(built.Opcode, built.Body);

        Received raw = await _client.ReceiveAsync(s_wait);
        Assert.Equal(built.Opcode, raw.Opcode);
        Assert.Equal(built.Body, raw.Body);
        Assert.Equal(5u, Assert.IsType<LoginAccepted>(raw.Message).DroppedForLag);
    }

    [Fact]
    public async Task TheTapSeesEverySentMessageInOrderWithTheBodyTheClientReceived()
    {
        var tap = new RecordingTap();
        _connection.Tap = tap;

        _connection.Send(new LoginAccepted { DroppedForLag = 3 });
        Received first = await _client.ReceiveAsync(s_wait);
        _connection.SendRaw(first.Opcode, first.Body);
        Received second = await _client.ReceiveAsync(s_wait);

        await Eventually.TrueAsync(() => tap.SentMessages.Count == 2);
        (ushort Opcode, Type Type)[] expected = [(OpcodeOf<LoginAccepted>(), typeof(LoginAccepted)), (OpcodeOf<LoginAccepted>(), typeof(InjectedBody))];
        Assert.Equal(expected, tap.SentMessages.Select(s => (s.Opcode, s.Type)).ToArray());
        Assert.Equal(first.Body, tap.SentMessages.First().Body);
        Assert.Equal(second.Body, tap.SentMessages.Last().Body);
    }

    [Fact]
    public async Task TheTapSeesAReceivedFrameTheTableDrops()
    {
        var tap = new RecordingTap();
        _connection.Tap = tap;

        await _client.SendUncheckedAsync(0x0999, new RawBody([0x01]));

        await Eventually.TrueAsync(() => !tap.ReceivedFrames.IsEmpty);
        (ushort opcode, InboundResult result) = tap.ReceivedFrames.Single();
        Assert.Equal((ushort)0x0999, opcode);
        Assert.Equal(InboundResult.Dropped, result);
    }

    [Fact]
    public async Task ATapThatThrowsIsDetachedAndTheConnectionGoesOn()
    {
        _connection.Tap = new ThrowingTap();

        _connection.Send(new LoginAccepted { DroppedForLag = 1 });
        _connection.Send(new LoginAccepted { DroppedForLag = 2 });

        Assert.Equal(1u, (await _client.ExpectAsync<LoginAccepted>(s_wait)).DroppedForLag);
        Assert.Equal(2u, (await _client.ExpectAsync<LoginAccepted>(s_wait)).DroppedForLag);
        await Eventually.TrueAsync(() => _connection.Tap is null);
    }

    [Fact]
    public void UntapDetachesOnlyTheTapStillInPlace()
    {
        var old = new RecordingTap();
        var current = new RecordingTap();
        _connection.Tap = current;

        Assert.False(_connection.Untap(old));
        Assert.Same(current, _connection.Tap);
        Assert.True(_connection.Untap(current));
        Assert.Null(_connection.Tap);
    }

    private static ushort OpcodeOf<T>()
        where T : IWireMessage => T.Opcode;

    private sealed class RecordingTap : ISessionTap
    {
        public ConcurrentQueue<(ushort Opcode, byte[] Body, Type Type)> SentMessages { get; } = new();

        public ConcurrentQueue<(ushort Opcode, InboundResult Result)> ReceivedFrames { get; } = new();

        public void Received(long session, ReadOnlySpan<byte> envelope, Inbound inbound) => ReceivedFrames.Enqueue((inbound.Opcode, inbound.Result));

        public void Sent(long session, ushort opcode, ReadOnlySpan<byte> body, Type messageType) => SentMessages.Enqueue((opcode, body.ToArray(), messageType));
    }

    private sealed class ThrowingTap : ISessionTap
    {
        public void Received(long session, ReadOnlySpan<byte> envelope, Inbound inbound) => throw new InvalidOperationException("a tap bug");

        public void Sent(long session, ushort opcode, ReadOnlySpan<byte> body, Type messageType) => throw new InvalidOperationException("a tap bug");
    }

    // Every row is handled in every state, so only the decoder refuses a message here.
    private sealed class TapConnection : ClientConnection
    {
        public TapConnection(TimeProvider time, GatewayOptions options)
            : base(0x0076, MessageCatalog.Default, new InboundDecoder(MessageCatalog.Default, OpcodeTable.Pregame, new HashSet<ushort>()),
                Options.Create(options), time, new GatewayMetrics(), NullLogger.Instance)
        {
        }

        protected override bool Admits(OpcodeRow row) => true;

        protected override ValueTask<bool> HandleAsync(IWireMessage message, OpcodeRow row) => ValueTask.FromResult(true);
    }
}
#endif
