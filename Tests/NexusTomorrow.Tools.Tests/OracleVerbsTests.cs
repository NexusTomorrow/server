using System.Collections.Concurrent;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading.Channels;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.Connections;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Tools.Tests;

public sealed class OracleVerbsTests : IAsyncLifetime
{
    private static readonly TimeSpan s_wait = TimeSpan.FromSeconds(10);
    private static readonly AccountCredentials s_alice = new(7, "alice@example.com", [1], [2], SuspendedUntil: null, SuspendedIndefinitely: false);

    private readonly AccountOwner _accounts =
        new(new FakeTimeProvider(), Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance, new FakeWriteBehind());
    private readonly SessionDouble _session = new();
    private readonly Channel<string> _lines = Channel.CreateUnbounded<string>();
    private readonly OracleConnection _connection;
    private OracleVerbs _verbs = null!;

    public OracleVerbsTests()
    {
        _connection = new OracleConnection(_lines.Writer);
    }

    public async ValueTask InitializeAsync()
    {
        _accounts.Start();
        _verbs = new OracleVerbs(_accounts, new FakeTimeProvider(new DateTimeOffset(2026, 10, 23, 12, 0, 0, TimeSpan.Zero)));
        await LogInAsync(s_alice, _session);
    }

    public async ValueTask DisposeAsync()
    {
        _connection.Dispose();
        await _accounts.StopAsync().WaitAsync(s_wait);
    }

    [Fact]
    public async Task InjectQueuesTheBuiltMessageOnTheAccountsSessionBelowTheGate()
    {
        JsonObject reply = await RequestAsync("inject", """{"account": "ALICE@example.com", "opcode": "0x036A", "fields": {"Reason": 3}}""");

        Assert.True((bool)reply["ok"]!);
        Assert.Equal("Kicked", (string)reply["type"]!);
        (ushort opcode, IWireStruct message) = Assert.Single(_session.Unchecked);
        Assert.Equal(0x036A, opcode);
        Assert.Equal(3, Assert.IsType<Kicked>(message).Reason);
        Assert.Equal(Convert.ToHexString(Serialize(message)), (string)reply["body"]!);
    }

    [Fact]
    public async Task InjectSendsABodyExactlyAsGiven()
    {
        JsonObject reply = await RequestAsync("inject", """{"account": "alice@example.com", "opcode": "0x036A", "body": "01FF"}""");

        Assert.True((bool)reply["ok"]!);
        (ushort opcode, byte[] body) = Assert.Single(_session.Raw);
        Assert.Equal((ushort)0x036A, opcode);
        Assert.Equal([0x01, 0xFF], body);
    }

    [Theory]
    [InlineData("""{"account": "bob@example.com", "opcode": "0x036A", "fields": {"Reason": 3}}""", "bob@example.com has no world session")]
    [InlineData("""{"account": "alice@example.com", "opcode": "36A", "fields": {"Reason": 3}}""", "inject names an opcode as 0xNNNN")]
    [InlineData("""{"account": "alice@example.com", "opcode": "0x036A", "fields": {}, "body": "00"}""", "inject takes either fields or body")]
    [InlineData("""{"account": "alice@example.com", "opcode": "0x036A", "fields": {}}""", "Reason: no value given")]
    [InlineData("""{"opcode": "0x036A", "fields": {"Reason": 3}}""", "inject names an account")]
    public async Task InjectNamesWhyItRefusesAndQueuesNothing(string request, string error)
    {
        JsonObject reply = await RequestAsync("inject", request);

        Assert.False((bool)reply["ok"]!);
        Assert.Equal(error, (string)reply["error"]!);
        Assert.Empty(_session.Unchecked);
        Assert.Empty(_session.Raw);
    }

    [Fact]
    public async Task TapNumbersEachDirectionFromOneAndDetachesWithItsConnection()
    {
        JsonObject reply = await RequestAsync("tap", """{"account": "alice@example.com"}""");
        Assert.True((bool)reply["ok"]!);
        SessionRecorder tap = Assert.IsType<SessionRecorder>(_session.Tap);

        tap.Sent(1, 0x036A, [0x01, 0x02], typeof(Kicked));
        tap.Received(1, [0xAA], new Inbound(InboundResult.Dropped, 0x0999, null, "no opcode table row"));
        tap.Sent(1, 0x036A, [0x03], typeof(Kicked));

        JsonObject[] records = [.. Records(3)];
        Assert.Equal([("s2c", 1L), ("c2s", 1L), ("s2c", 2L)], records.Select(r => ((string)r["dir"]!, (long)r["n"]!)));
        Assert.Equal("0102", (string)records[0]["body"]!);
        Assert.Equal("0x0999", (string)records[1]["opcode"]!);
        Assert.Equal("Dropped", (string)records[1]["result"]!);
        Assert.Equal("AA", (string)records[1]["envelope"]!);
        Assert.Equal(new DateTimeOffset(2026, 10, 23, 12, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds(), (long)records[0]["t"]!);

        _connection.Dispose();
        Assert.Null(_session.Tap);
    }

    [Fact]
    public async Task ATappedMessageThatCarriesASecretIsRecordedWithoutItsBytes()
    {
        await RequestAsync("tap", """{"account": "alice@example.com"}""");
        ISessionTap tap = _session.Tap!;

        tap.Sent(1, 0x07A1, [0x01], typeof(RealmRedirect));
        tap.Received(1, [0x02], new Inbound(InboundResult.Message, 0x058F, new WorldHello(), ""));
        tap.Received(1, [0x03], new Inbound(InboundResult.Malformed, 0x058F, null, "bits left"));

        JsonObject[] records = [.. Records(3)];
        Assert.All(records, r => Assert.True((bool)r["redacted"]!));
        Assert.Null(records[0]["body"]);
        Assert.Null(records[1]["envelope"]);
        Assert.Null(records[1]["fields"]);
        Assert.Null(records[2]["envelope"]);
        Assert.True(SessionRecorder.HoldsSecret(typeof(RealmRedirect)));
        Assert.False(SessionRecorder.HoldsSecret(typeof(Kicked)));
    }

    [Fact]
    public async Task AReceivedMessageIsRecordedWithItsDecodedFields()
    {
        await RequestAsync("tap", """{"account": "alice@example.com"}""");
        var receipt = new Network.World.Message.Model.Movement.MovementControlReceipt();
        MessageBinder.Bind(receipt, new JsonObject { ["Ticket"] = 77 });

        _session.Tap!.Received(1, [0x01], new Inbound(InboundResult.Message, 0x0635, receipt, ""));

        JsonObject record = Assert.Single(Records(1));
        Assert.Equal("""{"Ticket":77}""", record["fields"]!.ToJsonString());
    }

    [Fact]
    public async Task AVerbThatFailsOnAnErrorItDoesNotNameIsAnsweredWithIt()
    {
        // The union's case is parsed as a decimal, which takes no exponent.
        JsonObject reply = await RequestAsync(
            "inject", """{"account": "alice@example.com", "opcode": "0x0247", "fields": {"Type": 0, "Payload": {"$case": 1e400}}}""");

        Assert.False((bool)reply["ok"]!);
        Assert.StartsWith("inject failed: FormatException", (string)reply["error"]!);
        Assert.Empty(_session.Unchecked);
    }

    [Theory]
    [InlineData(typeof(RealmRedirect))]
    [InlineData(typeof(WorldHello))]
    [InlineData(typeof(Network.Auth.Message.Model.RealmHandoff))]
    [InlineData(typeof(Network.Auth.Message.Model.LoginRequest))]
    [InlineData(typeof(Network.World.Message.Model.Chat.ChatJoinChannel))]
    [InlineData(typeof(Network.World.Message.Model.Chat.ChatChannelSetPassword))]
    public void EveryMessageCarryingAKeyATokenOrAPasswordIsRecordedWithoutItsBytes(Type message)
    {
        Assert.True(SessionRecorder.HoldsSecret(message));
    }

    [Fact]
    public async Task LayoutListsTheMembersInWireOrder()
    {
        JsonObject reply = await RequestAsync("layout", """{"opcode": "0x036A"}""");

        Assert.True((bool)reply["ok"]!);
        JsonObject member = Assert.IsType<JsonObject>(Assert.Single(Assert.IsType<JsonArray>(reply["members"])));
        Assert.Equal(("Reason", "5", "unsigned"), ((string)member["path"]!, (string)member["bits"]!, (string)member["encoding"]!));
    }

    private async Task<JsonObject> RequestAsync(string verb, string request)
    {
        var body = (JsonObject)JsonNode.Parse(request)!;
        body["verb"] = verb;
        body["id"] = 1;
        return await _verbs.HandleAsync(body, _connection, TestContext.Current.CancellationToken);
    }

    private IEnumerable<JsonObject> Records(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Assert.True(_lines.Reader.TryRead(out string? line));
            yield return Assert.IsType<JsonObject>(JsonNode.Parse(line!)!["tap"]);
        }
    }

    private async Task LogInAsync(AccountCredentials account, IClientSession session)
    {
        var token = new TaskCompletionSource<Guid>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_accounts.Post(new IssueGameToken(account, token)));
        var login = new TaskCompletionSource<LoginDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_accounts.Post(new RedeemGameToken(await token.Task.WaitAsync(s_wait), account.Email, login)));
        LoginGranted grant = Assert.IsType<LoginGranted>(await login.Task.WaitAsync(s_wait));
        var world = new TaskCompletionSource<WorldDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_accounts.Post(new RedeemSessionKey(grant.SessionKey, grant.Echo, account.Email, session, world)));
        Assert.IsType<WorldGranted>(await world.Task.WaitAsync(s_wait));
    }

    private static byte[] Serialize(IWireStruct message)
    {
        byte[] rented = Transport.FrameWriter.RentBody(message, out int length);
        byte[] body = rented.AsSpan(0, length).ToArray();
        System.Buffers.ArrayPool<byte>.Shared.Return(rented, clearArray: true);
        return body;
    }

    // A world session that records what the oracle queues on it.
    private sealed class SessionDouble : IClientSession, IDebugSession
    {
        public ConcurrentQueue<(ushort Opcode, IWireStruct Message)> Unchecked { get; } = new();

        public ConcurrentQueue<(ushort Opcode, byte[] Body)> Raw { get; } = new();

        public long Id => 1;

        public IPAddress RemoteAddress => IPAddress.Loopback;

        private ISessionTap? _tap;

        public ISessionTap? Tap
        {
            get => _tap;
            set => _tap = value;
        }

        public bool Untap(ISessionTap tap) => Interlocked.CompareExchange(ref _tap, null, tap) == tap;

        public void Send<T>(T message)
            where T : class, IWireMessage
        {
        }

        public void Disconnect(string reason)
        {
        }

        public void SendUnchecked(ushort opcode, IWireStruct message) => Unchecked.Enqueue((opcode, message));

        public void SendRaw(ushort opcode, ReadOnlySpan<byte> body) => Raw.Enqueue((opcode, body.ToArray()));
    }
}
