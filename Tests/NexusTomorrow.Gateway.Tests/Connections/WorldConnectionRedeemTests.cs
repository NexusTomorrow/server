using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.Connections;
using NexusTomorrow.Network;
using NexusTomorrow.Network.Message.Model;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.Text;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Tests.Connections;

public sealed class WorldConnectionRedeemTests : IAsyncLifetime
{
    private static readonly AccountCredentials s_alice = new(7, "alice@example.com", [1], [2], null, false);

    private readonly FakeTimeProvider _time = new();
    private readonly CapturingLogger _log = new();
    private readonly AccountOwner _accounts;
    private readonly RealmOwner _realm;
    private readonly PipePair _pipes = new();
    private readonly CancellationTokenSource _stop = new();
    private Task _run = Task.CompletedTask;

    public WorldConnectionRedeemTests()
    {
        _accounts = new AccountOwner(_time, Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance, new FakeWriteBehind(), FixedEntitlementCaps.Defaults());
        _realm = new RealmOwner(new FakeWriteBehind(), new FixedCreationCatalog([]), new WordFilter([], NullLogger<WordFilter>.Instance), FixedEntitlementCaps.Defaults(), new FakeMapDirectory(), _time, Options.Create(new RealmOptions { Id = 3 }), NullLogger<RealmOwner>.Instance);
    }

    private GameTestClient Client { get; set; } = null!;

    public ValueTask InitializeAsync()
    {
        _accounts.Start();
        _realm.Post(new IdBlockTaken(new IdBlock(1001, 1000)));
        _realm.Start();
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _stop.CancelAsync();
        await _run.WaitAsync(TimeSpan.FromSeconds(10));
        await _realm.StopAsync().WaitAsync(TimeSpan.FromSeconds(10));
        await _accounts.StopAsync().WaitAsync(TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task AFaultedAccountOwnerReplyEndsTheConnectionAndIsLogged()
    {
        Start((_, _, _) => Task.FromException<WorldDecision>(new InvalidOperationException("the owner broke")));
        await Client.ExpectAsync<ServerHello>();
        await Client.SendAsync(Hello(new LoginGranted(7, new byte[16], 0)));

        Assert.True(await Client.IsClosedAsync(TimeSpan.FromSeconds(10)));
        await Eventually.TrueAsync(() => _log.Lines.Any(line => line.Contains("the account owner failed", StringComparison.Ordinal)));
    }

    [Fact]
    public async Task AReplyThatFaultsWithCancellationWhileTheConnectionLivesIsAnOwnerFailure()
    {
        Start((_, _, _) => Task.FromCanceled<WorldDecision>(new CancellationToken(true)));
        await Client.ExpectAsync<ServerHello>();
        await Client.SendAsync(Hello(new LoginGranted(7, new byte[16], 0)));

        Assert.True(await Client.IsClosedAsync(TimeSpan.FromSeconds(10)));
        await Eventually.TrueAsync(() => _log.Lines.Any(line => line.Contains("the account owner failed", StringComparison.Ordinal)));
    }

    [Fact]
    public async Task AGrantThatLandsAfterTheConnectionClosedTakesTheAccountOffline()
    {
        var gate = new TaskCompletionSource<WorldDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        var owned = new TaskCompletionSource<WorldDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        Start((connection, hello, key) =>
        {
            _accounts.Post(new RedeemSessionKey(key, hello.AccountId, hello.AccountName, connection, owned));
            return gate.Task;
        });
        await Client.ExpectAsync<ServerHello>();
        await Client.SendAsync(Hello(await GrantAsync()));

        Assert.IsType<WorldGranted>(await owned.Task.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken));
        await Eventually.TrueAsync(() => _accounts.OnlineCount == 1);
        await _stop.CancelAsync();
        await _run.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        Assert.Equal(1, _accounts.OnlineCount);

        gate.SetResult(await owned.Task);
        await Eventually.TrueAsync(() => _accounts.OnlineCount == 0);
    }

    private void Start(WorldConnection.SessionKeyRedeemer redeemer)
    {
        IOptions<GatewayOptions> gateway = Options.Create(new GatewayOptions { UnknownAccountSecret = Convert.ToBase64String(new byte[32]) });
        var connection = new WorldConnection(MessageCatalog.Default, new InboundDecoder(MessageCatalog.Default, OpcodeTable.Pregame, new HashSet<ushort>()),
            _accounts, _realm, gateway, Options.Create(new RealmOptions { Id = 3 }), _time, new GatewayMetrics(), _log, redeemer);
        _run = connection.RunAsync(_pipes.Server, new IPEndPoint(IPAddress.Loopback, 50003), _stop.Token);
        Client = new GameTestClient(_pipes.Client);
    }

    private async Task<LoginGranted> GrantAsync()
    {
        var issued = new TaskCompletionSource<Guid>(TaskCreationOptions.RunContinuationsAsynchronously);
        _accounts.Post(new IssueGameToken(s_alice, issued));
        var granted = new TaskCompletionSource<LoginDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        _accounts.Post(new RedeemGameToken(await issued.Task.WaitAsync(TimeSpan.FromSeconds(10)), "alice@example.com", granted));
        return Assert.IsType<LoginGranted>(await granted.Task.WaitAsync(TimeSpan.FromSeconds(10)));
    }

    private static WorldHello Hello(LoginGranted grant)
    {
        var hello = new WorldHello { AccountId = grant.Echo, AccountName = "alice@example.com" };
        var key = NetworkGuid.FromBytes(grant.SessionKey);
        hello.SessionKey.Data1 = key.Data1;
        hello.SessionKey.Data2 = key.Data2;
        hello.SessionKey.Data3 = key.Data3;
        hello.SessionKey.Data4 = key.Data4;
        return hello;
    }

    private sealed class CapturingLogger : ILogger<WorldConnection>
    {
        private readonly List<string> _lines = [];
        private readonly Lock _gate = new();

        public IReadOnlyList<string> Lines
        {
            get
            {
                lock (_gate)
                    return [.. _lines];
            }
        }

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            lock (_gate)
                _lines.Add(formatter(state, exception));
        }
    }
}
