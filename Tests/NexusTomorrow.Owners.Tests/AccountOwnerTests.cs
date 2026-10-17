using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Owners.Tests;

public sealed class AccountOwnerTests : IAsyncLifetime
{
    private static readonly AccountCredentials s_alice = new(7, "alice@example.com", [1], [2], SuspendedUntil: null, SuspendedIndefinitely: false);

    private readonly FakeTimeProvider _time = new(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));
    private readonly AccountOwner _owner;

    public AccountOwnerTests()
    {
        _owner = new AccountOwner(_time, Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance);
        _owner.Start();
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync() => await _owner.StopAsync().WaitAsync(TimeSpan.FromSeconds(10));

    [Fact]
    public async Task AGameTokenWorksOnce()
    {
        Guid token = await IssueAsync(s_alice);
        Assert.IsType<LoginGranted>(await RedeemAsync(token, "alice@example.com"));
        Assert.Equal(LoginResults.General, Assert.IsType<LoginRefused>(await RedeemAsync(token, "alice@example.com")).Result);
    }

    [Fact]
    public async Task AGameTokenExpiresAfterItsLifetime()
    {
        Guid token = await IssueAsync(s_alice);
        _time.Advance(TimeSpan.FromSeconds(61));
        Assert.IsType<LoginRefused>(await RedeemAsync(token, "alice@example.com"));
    }

    [Fact]
    public async Task AGameTokenRedeemedUnderAnotherNameIsRefused()
    {
        Guid token = await IssueAsync(s_alice);
        Assert.IsType<LoginRefused>(await RedeemAsync(token, "mallory@example.com"));
    }

    [Fact]
    public async Task TheAccountNameIsComparedWithoutRegardToCase()
    {
        Guid token = await IssueAsync(s_alice);
        Assert.IsType<LoginGranted>(await RedeemAsync(token, "Alice@Example.com"));
    }

    [Fact]
    public async Task ASuspensionInForceRefusesTheLogin()
    {
        Guid token = await IssueAsync(s_alice with { SuspendedUntil = _time.GetUtcNow().AddHours(1) });
        Assert.IsType<LoginRefused>(await RedeemAsync(token, "alice@example.com"));
    }

    [Fact]
    public async Task ASuspensionThatHasEndedDoesNotRefuse()
    {
        Guid token = await IssueAsync(s_alice with { SuspendedUntil = _time.GetUtcNow().AddMinutes(-1) });
        Assert.IsType<LoginGranted>(await RedeemAsync(token, "alice@example.com"));
    }

    [Fact]
    public async Task ASuspensionChangedWhileRunningOverridesTheCredentials()
    {
        _owner.Post(new SuspensionChanged(7, Until: null, Indefinite: true));
        Guid token = await IssueAsync(s_alice);
        Assert.IsType<LoginRefused>(await RedeemAsync(token, "alice@example.com"));

        _owner.Post(new SuspensionChanged(7, Until: null, Indefinite: false));
        token = await IssueAsync(s_alice with { SuspendedIndefinitely = true });
        Assert.IsType<LoginGranted>(await RedeemAsync(token, "alice@example.com"));
    }

    [Fact]
    public async Task AGrantCarriesSixteenFreshSessionKeyBytes()
    {
        LoginGranted first = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        LoginGranted second = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        Assert.Equal(16, first.SessionKey.Length);
        Assert.NotEqual(first.SessionKey, second.SessionKey);
    }

    [Fact]
    public async Task ASessionKeyWorksOnceAndChecksTheEchoAndName()
    {
        LoginGranted grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        var session = new FakeClientSession();
        Assert.IsType<WorldRefused>(await GreetAsync(grant.SessionKey, unchecked(grant.Echo + 1), "alice@example.com", session));
        Assert.IsType<WorldRefused>(await GreetAsync(grant.SessionKey, grant.Echo, "mallory@example.com", session));

        grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        Assert.Equal(7, Assert.IsType<WorldGranted>(await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", session)).AccountId);
        Assert.IsType<WorldRefused>(await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", session));
    }

    [Fact]
    public async Task ASessionKeyExpiresAfterItsLifetime()
    {
        LoginGranted grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        _time.Advance(TimeSpan.FromSeconds(61));
        Assert.IsType<WorldRefused>(await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", new FakeClientSession()));
    }

    [Fact]
    public async Task ASecondWorldLoginReplacesTheFirst()
    {
        var first = new FakeClientSession();
        var second = new FakeClientSession();
        LoginGranted grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", first);
        grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", second);

        Assert.Equal(0, first.Single<Kicked>().Reason);
        Assert.NotNull(first.DisconnectReason);
        Assert.Null(second.DisconnectReason);
        Assert.Equal(1, _owner.OnlineCount);
    }

    [Fact]
    public async Task ClosingAReplacedSessionLeavesTheNewOneOnline()
    {
        var first = new FakeClientSession();
        var second = new FakeClientSession();
        LoginGranted grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", first);
        grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", second);

        _owner.Post(new WorldSessionClosed(7, first));
        await IssueAsync(s_alice); // a round trip, so the close has been handled
        Assert.Equal(1, _owner.OnlineCount);
        _owner.Post(new WorldSessionClosed(7, second));
        await IssueAsync(s_alice);
        Assert.Equal(0, _owner.OnlineCount);
    }

    private async Task<Guid> IssueAsync(AccountCredentials account)
    {
        var reply = new TaskCompletionSource<Guid>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_owner.Post(new IssueGameToken(account, reply)));
        return await reply.Task.WaitAsync(TimeSpan.FromSeconds(10));
    }

    private async Task<LoginDecision> RedeemAsync(Guid token, string name)
    {
        var reply = new TaskCompletionSource<LoginDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_owner.Post(new RedeemGameToken(token, name, reply)));
        return await reply.Task.WaitAsync(TimeSpan.FromSeconds(10));
    }

    private async Task<WorldDecision> GreetAsync(byte[] key, uint echo, string name, FakeClientSession session)
    {
        var reply = new TaskCompletionSource<WorldDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_owner.Post(new RedeemSessionKey(key, echo, name, session, reply)));
        return await reply.Task.WaitAsync(TimeSpan.FromSeconds(10));
    }
}
