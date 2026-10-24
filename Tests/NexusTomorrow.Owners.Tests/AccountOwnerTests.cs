using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Owners.Tests;

public sealed class AccountOwnerTests : IAsyncLifetime
{
    private static readonly AccountCredentials s_alice = new(7, "alice@example.com", [1], [2], SuspendedUntil: null, SuspendedIndefinitely: false);

    private readonly FakeTimeProvider _time = new(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));
    private readonly FakeWriteBehind _db = new();
    private readonly AccountOwner _owner;

    public AccountOwnerTests()
    {
        _owner = new AccountOwner(_time, Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance, _db);
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
    public async Task ANewGameTokenSupersedesTheAccountsEarlierOne()
    {
        Guid first = await IssueAsync(s_alice);
        Guid second = await IssueAsync(s_alice);
        Assert.Equal(LoginResults.General, Assert.IsType<LoginRefused>(await RedeemAsync(first, "alice@example.com")).Result);
        Assert.IsType<LoginGranted>(await RedeemAsync(second, "alice@example.com"));
    }

    [Fact]
    public async Task TheOwnerHoldsOneGameTokenPerAccount()
    {
        var bob = new AccountCredentials(8, "bob@example.com", [1], [2], SuspendedUntil: null, SuspendedIndefinitely: false);
        Guid bobs = await IssueAsync(bob);
        for (int i = 0; i < 100; i++)
            await IssueAsync(s_alice);
        Assert.Equal(2, _owner.PendingGameTokens);

        // Alice's tokens never superseded Bob's, and redeeming it frees his entry.
        Assert.IsType<LoginGranted>(await RedeemAsync(bobs, "bob@example.com"));
        Assert.Equal(1, _owner.PendingGameTokens);
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
    public async Task ASuspensionStoredWhileRunningOverridesTheCredentialsUntilLifted()
    {
        await StoreSuspensionAsync(s_alice.Email, until: null);
        Guid token = await IssueAsync(s_alice);
        Assert.IsType<LoginRefused>(await RedeemAsync(token, "alice@example.com"));

        await LiftAsync(s_alice.Email);
        token = await IssueAsync(s_alice with { SuspendedIndefinitely = true });
        Assert.IsType<LoginGranted>(await RedeemAsync(token, "alice@example.com"));
    }

    [Fact]
    public async Task ATimedSuspensionAddedToAnIndefiniteOneInTheDatabaseDoesNotShortenIt()
    {
        await StoreSuspensionAsync(s_alice.Email, _time.GetUtcNow().AddHours(1));
        _time.Advance(TimeSpan.FromHours(2));

        Guid token = await IssueAsync(s_alice with { SuspendedIndefinitely = true });
        Assert.IsType<LoginRefused>(await RedeemAsync(token, "alice@example.com"));
    }

    [Fact]
    public async Task AShorterTimedSuspensionDoesNotReplaceALongerOne()
    {
        await StoreSuspensionAsync(s_alice.Email, _time.GetUtcNow().AddHours(5));
        await StoreSuspensionAsync(s_alice.Email, _time.GetUtcNow().AddHours(1));
        _time.Advance(TimeSpan.FromHours(2));
        Assert.IsType<LoginRefused>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));

        _time.Advance(TimeSpan.FromHours(4));
        Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
    }

    [Fact]
    public async Task AnIndefiniteSuspensionReplacesATimedOne()
    {
        await StoreSuspensionAsync(s_alice.Email, _time.GetUtcNow().AddHours(1));
        await StoreSuspensionAsync(s_alice.Email, until: null);
        _time.Advance(TimeSpan.FromHours(2));
        Assert.IsType<LoginRefused>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
    }

    [Fact]
    public async Task ALiftEndsEverySuspensionCombinedSoFarAndALaterOneStartsFresh()
    {
        await StoreSuspensionAsync(s_alice.Email, until: null);
        await LiftAsync(s_alice.Email);
        Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice with { SuspendedIndefinitely = true }), "alice@example.com"));

        await StoreSuspensionAsync(s_alice.Email, _time.GetUtcNow().AddHours(1));
        _time.Advance(TimeSpan.FromHours(2));
        Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice with { SuspendedIndefinitely = true }), "alice@example.com"));
    }

    [Fact]
    public async Task ATimedSuspensionEndsWhenItsTimeComes()
    {
        await StoreSuspensionAsync(s_alice.Email, _time.GetUtcNow().AddHours(1));
        Assert.IsType<LoginRefused>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));

        _time.Advance(TimeSpan.FromHours(2));
        Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
    }

    [Fact]
    public async Task ASuspensionLandingBetweenTheTokenStepAndTheWorldStepRefusesTheSessionKey()
    {
        LoginGranted grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        await StoreSuspensionAsync(s_alice.Email, until: null);

        var session = new FakeClientSession();
        Assert.IsType<WorldRefused>(await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", session));
        Assert.Equal(0, _owner.OnlineCount);
    }

    [Fact]
    public async Task ASuspensionKicksTheWorldSessionOfTheAccount()
    {
        var session = new FakeClientSession();
        LoginGranted grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", session);

        await StoreSuspensionAsync(s_alice.Email, until: null);

        Assert.Equal(0, session.Single<Kicked>().Unknown0);
        Assert.NotNull(session.DisconnectReason);
        Assert.Equal(0, _owner.OnlineCount);
    }

    [Fact]
    public async Task ASuspensionOfAnUnknownEmailChangesNothing()
    {
        var reply = new TaskCompletionSource<AdminResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_owner.Post(new SuspendAccount("nobody@example.com", Until: null, "test", "test", reply)));
        await WaitForAsync<SuspendAccountWork>();
        _db.Take<SuspendAccountWork>().Complete(WorkResult.Committed);

        AdminResult result = await reply.Task.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);
        Assert.False(result.Succeeded);
        Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
    }

    [Fact]
    public async Task ACreatedAccountIsReportedOnlyOnceItIsCommitted()
    {
        var reply = new TaskCompletionSource<AdminResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_owner.Post(new CreateAccount("bob@example.com", [1], [2], reply)));
        await WaitForAsync<CreateAccountWork>();
        Assert.False(reply.Task.IsCompleted);

        _db.Take<CreateAccountWork>().Complete(WorkResult.Committed);
        Assert.True((await reply.Task.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken)).Succeeded);
    }

    [Fact]
    public async Task ATakenEmailIsReportedAsRefused()
    {
        var reply = new TaskCompletionSource<AdminResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_owner.Post(new CreateAccount("bob@example.com", [1], [2], reply)));
        await WaitForAsync<CreateAccountWork>();

        _db.Take<CreateAccountWork>().Complete(WorkResult.RefusedBy("ix_account_email"));
        Assert.False((await reply.Task.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken)).Succeeded);
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

        Assert.Equal(0, first.Single<Kicked>().Unknown0);
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

    [Fact]
    public async Task AnOnlineSessionIsFoundByItsAccountsEmailWithoutRegardToCase()
    {
        var session = new FakeClientSession();
        LoginGranted grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", session);

        Assert.Same(session, await FindAsync("ALICE@example.com"));
    }

    [Fact]
    public async Task NoSessionIsFoundForAnAccountThatIsNotOnline()
    {
        var session = new FakeClientSession();
        Assert.Null(await FindAsync("alice@example.com"));
        LoginGranted grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", session);

        _owner.Post(new WorldSessionClosed(7, session));
        Assert.Null(await FindAsync("alice@example.com"));
        Assert.Null(await FindAsync("bob@example.com"));
    }

    [Fact]
    public async Task TheSessionThatReplacedAnotherIsTheOneFound()
    {
        var first = new FakeClientSession();
        var second = new FakeClientSession();
        LoginGranted grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", first);
        grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync(s_alice), "alice@example.com"));
        await GreetAsync(grant.SessionKey, grant.Echo, "alice@example.com", second);

        Assert.Same(second, await FindAsync("alice@example.com"));
    }

    private async Task<IClientSession?> FindAsync(string email)
    {
        var reply = new TaskCompletionSource<IClientSession?>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_owner.Post(new FindOnlineSession(email, reply)));
        return await reply.Task.WaitAsync(TimeSpan.FromSeconds(10));
    }

    private async Task StoreSuspensionAsync(string email, DateTimeOffset? until)
    {
        var reply = new TaskCompletionSource<AdminResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_owner.Post(new SuspendAccount(email, until, "test", "test", reply)));
        await WaitForAsync<SuspendAccountWork>();
        SuspendAccountWork work = _db.Take<SuspendAccountWork>();
        work.AccountId = s_alice.Id;
        work.Complete(WorkResult.Committed);
        Assert.True((await reply.Task.WaitAsync(TimeSpan.FromSeconds(10))).Succeeded);
    }

    private async Task LiftAsync(string email)
    {
        var reply = new TaskCompletionSource<AdminResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        Assert.True(_owner.Post(new LiftSuspension(email, reply)));
        await WaitForAsync<LiftSuspensionWork>();
        LiftSuspensionWork work = _db.Take<LiftSuspensionWork>();
        work.AccountId = s_alice.Id;
        work.Complete(WorkResult.Committed);
        Assert.True((await reply.Task.WaitAsync(TimeSpan.FromSeconds(10))).Succeeded);
    }

    private Task WaitForAsync<T>()
        where T : PersistenceWork => Eventually.TrueAsync(() => _db.Queued.OfType<T>().Any());

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
