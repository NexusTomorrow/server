using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.Connections;
using NexusTomorrow.Gateway.Limits;
using NexusTomorrow.Gateway.Tests.Sts;
using NexusTomorrow.Network;
using NexusTomorrow.Network.Auth.Message.Model;
using NexusTomorrow.Network.Message.Model;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Testing;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Tests.Connections;

public sealed class LoginConnectionTests : IAsyncLifetime
{
    private static readonly AccountCredentials s_alice = new(7, "alice@example.com", [1], [2], null, false);

    private readonly FakeTimeProvider _time = new();
    private readonly FakeWriteBehind _persistence = new();
    private readonly AccountOwner _accounts;
    private readonly IOptions<GatewayOptions> _gateway = Options.Create(new GatewayOptions { UnknownAccountSecret = Convert.ToBase64String(new byte[32]) });
    private readonly RecordingLogger<LoginConnection> _log = new();
    private readonly PipePair _pipes = new();
    private readonly CancellationTokenSource _stop = new();
    private Task _run = Task.CompletedTask;

    public LoginConnectionTests()
    {
        _accounts = new AccountOwner(_time, Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance, new FakeWriteBehind());
    }

    private GameTestClient Client { get; set; } = null!;

    public ValueTask InitializeAsync()
    {
        _accounts.Start();
        var connection = new LoginConnection(MessageCatalog.Default,
            new InboundDecoder(MessageCatalog.Default, OpcodeTable.All, new HashSet<ushort>()),
            _accounts, _persistence, new LoginRateLimiter(_time, _gateway), _gateway, Options.Create(new RealmOptions()),
            _time, new GatewayMetrics(), _log);
        _run = connection.RunAsync(_pipes.Server, new IPEndPoint(IPAddress.Loopback, 50000), _stop.Token);
        Client = new GameTestClient(_pipes.Client);
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _stop.CancelAsync();
        await _run.WaitAsync(TimeSpan.FromSeconds(10));
        await _accounts.StopAsync();
    }

    [Fact]
    public async Task TheConnectionGreetsBareWithTheBuildAndProtocolVersion()
    {
        Received greeting = await Client.ReceiveAsync();
        Assert.Equal(0x0003, greeting.FrameOpcode);
        ServerHello hello = Assert.IsType<ServerHello>(greeting.Message);
        Assert.Equal(16042u, hello.Build);
        Assert.Equal(0x097998A0u, hello.ProtocolVersion);
    }

    [Fact]
    public async Task AValidTokenGetsAcceptedThenAHandoffTheAccountOwnerHonours()
    {
        await Client.ExpectAsync<ServerHello>();
        await Client.SendAsync(Request(await IssueAsync()));

        Received accepted = await Client.ReceiveAsync();
        Assert.Equal(0x0076, accepted.FrameOpcode);
        Assert.IsType<LoginAccepted>(accepted.Message);
        RealmHandoff handoff = await Client.ExpectAsync<RealmHandoff>();
        Assert.Equal(0x7F000001u, handoff.WorldAddress);
        Assert.Equal(24000, handoff.WorldPort);
        Assert.Equal("NexusTomorrow", handoff.RealmName);

        byte[] key = new byte[16];
        handoff.SessionKey.CopyTo(key);
        var reply = new TaskCompletionSource<WorldDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        _accounts.Post(new RedeemSessionKey(key, handoff.Echo, "alice@example.com", new FakeClientSession(), reply));
        Assert.Equal(7, Assert.IsType<WorldGranted>(await reply.Task.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken)).AccountId);
    }

    [Fact]
    public async Task TheSessionKeyInTheHandoffReachesNoLogLineAsHexOrBase64()
    {
        await Client.ExpectAsync<ServerHello>();
        await Client.SendAsync(Request(await IssueAsync()));
        await Client.ExpectAsync<LoginAccepted>();
        RealmHandoff handoff = await Client.ExpectAsync<RealmHandoff>();
        byte[] key = new byte[16];
        handoff.SessionKey.CopyTo(key);

        IReadOnlyList<string> entries = _log.Entries;
        Assert.Contains(entries, entry => entry.Contains("RealmHandoff", StringComparison.Ordinal));
        string[] forbidden =
        [
            Convert.ToHexString(key),
            Convert.ToBase64String(key),
            new Guid(key).ToString(),
            new Guid(key).ToString("N"),
        ];
        foreach (string entry in entries)
        {
            foreach (string secret in forbidden)
                Assert.DoesNotContain(secret, entry, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public async Task AnUnknownTokenIsDeniedWithTheGeneralResult()
    {
        await Client.ExpectAsync<ServerHello>();
        await Client.SendAsync(Request(Guid.NewGuid()));
        Assert.Equal(LoginResults.General, (await Client.ExpectAsync<LoginDenied>()).Result);
    }

    [Fact]
    public async Task AnotherBuildIsDenied()
    {
        await Client.ExpectAsync<ServerHello>();
        LoginRequest request = Request(await IssueAsync());
        request.Build = 16041;
        await Client.SendAsync(request);
        await Client.ExpectAsync<LoginDenied>();
    }

    [Fact]
    public async Task LoginsAreDeniedWhileTheDatabaseIsUnavailable()
    {
        _persistence.IsAvailable = false;
        await Client.ExpectAsync<ServerHello>();
        await Client.SendAsync(Request(await IssueAsync()));
        await Client.ExpectAsync<LoginDenied>();
    }

    // 0x03EF has no row in the opcode table, so it is dropped before its body is read.
    [Fact]
    public async Task AMessageWithNoRowIsDroppedAndTheLoginStillWorks()
    {
        await Client.ExpectAsync<ServerHello>();
        await Client.SendUncheckedAsync(0x03EF, new CharacterListRequest());
        await Client.SendAsync(Request(await IssueAsync()));
        await Client.ExpectAsync<LoginAccepted>();
    }

    [Fact]
    public async Task AWorldMessageOnTheLoginConnectionEndsIt()
    {
        await Client.ExpectAsync<ServerHello>();
        await Client.SendAsync(new CharacterListRequest());
        Assert.True(await Client.IsClosedAsync(TimeSpan.FromSeconds(10)));
    }

    [Fact]
    public async Task AMovementBeforeTheLoginEndsTheConnectionAsUnexpectedWithoutItsBodyRead()
    {
        await Client.ExpectAsync<ServerHello>();
        await Client.SendTaggedUncheckedAsync(0x03, 0x0637, new RawBody(ClientFrames.OverclaimingMovement()));

        Assert.True(await Client.IsClosedAsync(TimeSpan.FromSeconds(10)));
        await _run.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        Assert.Contains(_log.Entries, entry => entry.Contains("ended: a message other than one login request on the login connection", StringComparison.Ordinal));
    }

    [Fact]
    public async Task AFrameOverTheCapEndsTheConnection()
    {
        await Client.ExpectAsync<ServerHello>();
        await Client.SendRawAsync([0x00, 0x00, 0x10, 0x00, 0x44, 0x02]); // 1 MiB
        Assert.True(await Client.IsClosedAsync(TimeSpan.FromSeconds(10)));
    }

    [Fact]
    public async Task APingIsEchoedBare()
    {
        await Client.ExpectAsync<ServerHello>();
        await Client.SendRawAsync([0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05]);
        Received answer = await Client.ReceiveAsync();
        Assert.Equal(TransportOpcodes.PingAnswer, answer.FrameOpcode);
        Assert.Equal([0x05], answer.Body);
    }

    [Fact]
    public async Task AnIdleConnectionEndsAfterThePreAuthTimeout()
    {
        await Client.ExpectAsync<ServerHello>();
        _time.Advance(TimeSpan.FromSeconds(31));
        Assert.True(await Client.IsClosedAsync(TimeSpan.FromSeconds(10)));
    }

    private async Task<Guid> IssueAsync()
    {
        var reply = new TaskCompletionSource<Guid>(TaskCreationOptions.RunContinuationsAsynchronously);
        _accounts.Post(new IssueGameToken(s_alice, reply));
        return await reply.Task.WaitAsync(TimeSpan.FromSeconds(10));
    }

    private static LoginRequest Request(Guid token)
    {
        var request = new LoginRequest { Build = 16042, AccountName = "alice@example.com" };
        var wire = NetworkGuid.FromGuid(token);
        request.GameToken.Data1 = wire.Data1;
        request.GameToken.Data2 = wire.Data2;
        request.GameToken.Data3 = wire.Data3;
        request.GameToken.Data4 = wire.Data4;
        return request;
    }
}
