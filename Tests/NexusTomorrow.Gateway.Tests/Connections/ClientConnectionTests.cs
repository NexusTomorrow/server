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
    private readonly RecordingConnection _connection;
    private Task _run = Task.CompletedTask;

    public ClientConnectionTests()
    {
        _connection = new RecordingConnection(new FakeTimeProvider());
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
    public void SendingAMessageBelowFieldsThrows()
    {
        Assert.Throws<InvalidOperationException>(() => _connection.Send(new UnverifiedForTest()));
    }

    private void Start() =>
        _run = _connection.RunAsync(_pipes.Server, new IPEndPoint(IPAddress.Loopback, 50001), _stop.Token);

    // A delegate, because IWireMessage declares static abstract members and so cannot be a type argument of Func.
    private delegate bool MessageHandler(IWireMessage message);

    private sealed class RecordingConnection : ClientConnection
    {
        private int _closed;
        private int _started;

        public RecordingConnection(TimeProvider time)
            : base(0x0076, MessageCatalog.Default, new InboundDecoder(MessageCatalog.Default, OpcodeTable.Pregame, new HashSet<ushort>()),
                Options.Create(new GatewayOptions { UnknownAccountSecret = "" }), time, new GatewayMetrics(), NullLogger.Instance)
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
