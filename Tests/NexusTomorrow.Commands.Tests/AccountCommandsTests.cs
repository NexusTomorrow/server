using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Cryptography;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Persistence.Ops;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Commands.Tests;

public sealed class AccountCommandsTests : IAsyncLifetime
{
    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(10);
    private static readonly CommandActor s_console = new("console", FrontEnd.Cli, new HashSet<string> { PermissionKeys.All }, null);
    private static readonly CommandActor s_gameMaster = new("gm", FrontEnd.InGame, new HashSet<string> { PermissionKeys.All }, "192.0.2.2");
    private static readonly CommandActor s_player = new("player", FrontEnd.InGame, new HashSet<string>(), "192.0.2.1");
    private static readonly AccountCredentials s_alice = new(7, "alice@example.com", [1], [2], null, false);

    private readonly FakeTimeProvider _time = new(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));
    private readonly FakeWriteBehind _db = new();
    private readonly CapturingLogger<AccountOwner> _ownerLog = new();
    private readonly CapturingLogger<CommandDispatcher> _dispatcherLog = new();
    private readonly AccountOwner _accounts;
    private readonly CommandDispatcher _dispatcher;

    public AccountCommandsTests()
    {
        _accounts = new AccountOwner(_time, Options.Create(new AccountOptions()), _ownerLog, _db, FixedEntitlementCaps.Defaults());
        _dispatcher = new CommandDispatcher(AccountCommands.Create(_accounts, _time), _db, _time, _dispatcherLog);
    }

    public ValueTask InitializeAsync()
    {
        _accounts.Start();
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync() => await _accounts.StopAsync().WaitAsync(s_bound);

    [Fact]
    public async Task AccountCreateRepliesAfterTheCommit()
    {
        Task<CommandReply> pending = _dispatcher.DispatchAsync(s_console, "account create Alice@Example.com \"correct horse\"", CancellationToken.None);
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateAccountWork>().Any());
        Assert.False(pending.IsCompleted);
        CreateAccountWork work = _db.Take<CreateAccountWork>();
        Assert.Equal("Alice@Example.com", work.Email);
        Assert.Equal(16, work.Salt.Length);
        work.Complete(WorkResult.Committed);
        Assert.Equal(CommandOutcome.Done, (await pending.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);
    }

    [Fact]
    public async Task AccountCreateMakesTheVerifierForTheLowercasedEmailWithAFreshSalt()
    {
        Task<CommandReply> first = _dispatcher.DispatchAsync(s_console, "account create Alice@Example.com pw", CancellationToken.None);
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateAccountWork>().Any());
        CreateAccountWork a = _db.Take<CreateAccountWork>();
        a.Complete(WorkResult.Committed);
        await first.WaitAsync(s_bound, TestContext.Current.CancellationToken);

        Task<CommandReply> second = _dispatcher.DispatchAsync(s_console, "account create bob@example.com pw", CancellationToken.None);
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateAccountWork>().Any());
        CreateAccountWork b = _db.Take<CreateAccountWork>();
        b.Complete(WorkResult.Committed);
        await second.WaitAsync(s_bound, TestContext.Current.CancellationToken);

        Assert.Equal(Srp6Provider.GenerateVerifier(a.Salt, "alice@example.com", "pw"), a.Verifier);
        Assert.NotEqual(a.Salt, b.Salt);
    }

    [Fact]
    public async Task ATakenEmailIsRefused()
    {
        Task<CommandReply> pending = _dispatcher.DispatchAsync(s_console, "account create alice@example.com pw", CancellationToken.None);
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateAccountWork>().Any());
        _db.Take<CreateAccountWork>().Complete(WorkResult.RefusedBy("ix_account_email"));
        Assert.Equal(CommandOutcome.Refused, (await pending.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);
    }

    [Theory]
    [InlineData("account create not-an-email pw")]
    [InlineData("account create a@b.c \"\"")]
    public async Task AnInvalidEmailOrPasswordIsRefusedBeforeAnythingIsQueued(string line)
    {
        Assert.Equal(CommandOutcome.Refused, (await _dispatcher.DispatchAsync(s_console, line, CancellationToken.None)).Outcome);
        Assert.Empty(_db.Queued.OfType<CreateAccountWork>());
    }

    [Fact]
    public async Task ThePasswordReachesNeitherTheAuditNorAnyLogNorAnyReply()
    {
        const string Password = "tr0ub4dor-and-3";
        var replies = new List<CommandReply>
        {
            await DispatchCreateAsync($"account create alice@example.com {Password}", WorkResult.Committed),
            await DispatchCreateAsync($"account create alice@example.com {Password}", WorkResult.RefusedBy("ix_account_email")),
            await _dispatcher.DispatchAsync(s_console, $"account create not-an-email {Password}", CancellationToken.None),
            await _dispatcher.DispatchAsync(s_gameMaster, $"account create alice@example.com {Password} spilled", CancellationToken.None),
        };

        List<AuditRow> audits = [];
        while (_db.Queued.OfType<AuditWork>().Any())
            audits.Add(_db.Take<AuditWork>().Row);
        Assert.Equal(4, audits.Count);
        foreach (AuditRow audit in audits)
        {
            string everything = $"{audit.Actor}|{audit.FrontEnd}|{audit.Action}|{audit.Target}|{audit.Details}|{audit.RemoteAddress}|{audit.Outcome}";
            Assert.DoesNotContain(Password, everything, StringComparison.Ordinal);
        }

        Assert.DoesNotContain(replies, reply => reply.Text.Contains(Password, StringComparison.Ordinal));
        Assert.DoesNotContain(_ownerLog.Lines.Concat(_dispatcherLog.Lines), line => line.Contains(Password, StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task APasswordInTheEmailSlotNeverReachesTheAudit(bool asConsole)
    {
        CommandActor actor = asConsole ? s_console : s_player;
        await _dispatcher.DispatchAsync(actor, "account create hunter2 a@b.c", CancellationToken.None);
        AuditRow audit = Assert.Single(_db.Queued.OfType<AuditWork>()).Row;
        string everything = $"{audit.Actor}|{audit.Action}|{audit.Target}|{audit.Details}|{audit.RemoteAddress}|{audit.Outcome}";
        Assert.DoesNotContain("hunter2", everything, StringComparison.Ordinal);
        Assert.Equal(asConsole ? "Refused" : "Denied", audit.Outcome);
    }

    [Fact]
    public async Task ALineWithAControlCharacterInTheEmailQueuesNothing()
    {
        CommandReply reply = await _dispatcher.DispatchAsync(s_console, "account create a\0@b.c pw", CancellationToken.None).WaitAsync(s_bound, TestContext.Current.CancellationToken);
        Assert.Equal(CommandOutcome.Refused, reply.Outcome);
        Assert.Empty(_db.Queued.OfType<CreateAccountWork>());
    }

    [Fact]
    public async Task ALineWithAControlCharacterInTheReasonQueuesNothing()
    {
        CommandReply reply = await _dispatcher.DispatchAsync(s_console, "account suspend alice@example.com forever bad\0reason", CancellationToken.None).WaitAsync(s_bound, TestContext.Current.CancellationToken);
        Assert.Equal(CommandOutcome.Refused, reply.Outcome);
        Assert.Empty(_db.Queued.OfType<SuspendAccountWork>());
    }

    [Fact]
    public async Task SuspendingForeverKicksTheWorldSessionAndRefusesTheNextLogin()
    {
        var session = new FakeClientSession();
        await OnlineAsync(session);

        Task<CommandReply> pending = _dispatcher.DispatchAsync(s_console, "account suspend alice@example.com forever spamming", CancellationToken.None);
        await Eventually.TrueAsync(() => _db.Queued.OfType<SuspendAccountWork>().Any());
        SuspendAccountWork work = _db.Take<SuspendAccountWork>();
        Assert.Null(work.EndsAt);
        Assert.Equal("spamming", work.Reason);
        work.AccountId = 7;
        work.Complete(WorkResult.Committed);
        Assert.Equal(CommandOutcome.Done, (await pending.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);

        await Eventually.TrueAsync(() => session.DisconnectReason is not null);
        Assert.Single(session.Sent.OfType<Kicked>());
        Assert.IsType<LoginRefused>(await RedeemAsync(await IssueAsync()));
    }

    [Fact]
    public async Task AReasonSpansTheRemainingTokens()
    {
        Task<CommandReply> pending = _dispatcher.DispatchAsync(s_console, "account suspend alice@example.com 1.5 spamming the \"trade channel\"", CancellationToken.None);
        await Eventually.TrueAsync(() => _db.Queued.OfType<SuspendAccountWork>().Any());
        SuspendAccountWork work = _db.Take<SuspendAccountWork>();
        Assert.Equal("spamming the trade channel", work.Reason);
        Assert.Equal(_time.GetUtcNow().AddHours(1.5), work.EndsAt);
        work.Complete(WorkResult.Committed);
        await pending.WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task SuspendingAnUnknownEmailIsRefused()
    {
        Task<CommandReply> pending = _dispatcher.DispatchAsync(s_console, "account suspend nobody@example.com 2", CancellationToken.None);
        await Eventually.TrueAsync(() => _db.Queued.OfType<SuspendAccountWork>().Any());
        SuspendAccountWork work = _db.Take<SuspendAccountWork>();
        Assert.Equal(_time.GetUtcNow().AddHours(2), work.EndsAt);
        work.Complete(WorkResult.Committed); // AccountId stays null: no such account
        Assert.Equal(CommandOutcome.Refused, (await pending.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);
    }

    [Theory]
    [InlineData("account suspend alice@example.com soon")]
    [InlineData("account suspend alice@example.com 0")]
    [InlineData("account suspend alice@example.com -3")]
    [InlineData("account suspend alice@example.com NaN")]
    [InlineData("account suspend alice@example.com 1e300")]
    public async Task AnUnusableDurationIsRefusedBeforeAnythingIsQueued(string line)
    {
        Assert.Equal(CommandOutcome.Refused, (await _dispatcher.DispatchAsync(s_console, line, CancellationToken.None)).Outcome);
        Assert.Empty(_db.Queued.OfType<SuspendAccountWork>());
    }

    [Fact]
    public async Task AReasonLongerThanItsColumnIsRefusedBeforeAnythingIsQueued()
    {
        CommandReply reply = await _dispatcher.DispatchAsync(s_console, $"account suspend alice@example.com forever {new string('x', 201)}", CancellationToken.None);
        Assert.Equal(CommandOutcome.Refused, reply.Outcome);
        Assert.Empty(_db.Queued.OfType<SuspendAccountWork>());
    }

    [Fact]
    public async Task SuspendingAndLiftingAreConsoleOnly()
    {
        Assert.Equal(CommandOutcome.Denied, (await _dispatcher.DispatchAsync(s_gameMaster, "account suspend alice@example.com forever", CancellationToken.None)).Outcome);
        Assert.Equal(CommandOutcome.Denied, (await _dispatcher.DispatchAsync(s_gameMaster, "account unsuspend alice@example.com", CancellationToken.None)).Outcome);
        Assert.Empty(_db.Queued.OfType<SuspendAccountWork>());
        Assert.Empty(_db.Queued.OfType<LiftSuspensionWork>());
    }

    [Fact]
    public async Task UnsuspendingLetsTheAccountLogInAgain()
    {
        await SuspendAsync();
        Assert.IsType<LoginRefused>(await RedeemAsync(await IssueAsync()));

        Task<CommandReply> pending = _dispatcher.DispatchAsync(s_console, "account unsuspend alice@example.com", CancellationToken.None);
        await Eventually.TrueAsync(() => _db.Queued.OfType<LiftSuspensionWork>().Any());
        LiftSuspensionWork work = _db.Take<LiftSuspensionWork>();
        work.AccountId = 7;
        work.Complete(WorkResult.Committed);
        Assert.Equal(CommandOutcome.Done, (await pending.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);
        Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync()));
    }

    [Fact]
    public async Task UnsuspendingAnUnknownEmailIsRefused()
    {
        Task<CommandReply> pending = _dispatcher.DispatchAsync(s_console, "account unsuspend nobody@example.com", CancellationToken.None);
        await Eventually.TrueAsync(() => _db.Queued.OfType<LiftSuspensionWork>().Any());
        _db.Take<LiftSuspensionWork>().Complete(WorkResult.Committed);
        Assert.Equal(CommandOutcome.Refused, (await pending.WaitAsync(s_bound, TestContext.Current.CancellationToken)).Outcome);
    }

    private async Task<CommandReply> DispatchCreateAsync(string line, WorkResult result)
    {
        Task<CommandReply> pending = _dispatcher.DispatchAsync(s_console, line, CancellationToken.None);
        await Eventually.TrueAsync(() => _db.Queued.OfType<CreateAccountWork>().Any());
        _db.Take<CreateAccountWork>().Complete(result);
        return await pending.WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }

    private async Task SuspendAsync()
    {
        Task<CommandReply> pending = _dispatcher.DispatchAsync(s_console, "account suspend alice@example.com forever", CancellationToken.None);
        await Eventually.TrueAsync(() => _db.Queued.OfType<SuspendAccountWork>().Any());
        SuspendAccountWork work = _db.Take<SuspendAccountWork>();
        work.AccountId = 7;
        work.Complete(WorkResult.Committed);
        await pending.WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }

    private async Task OnlineAsync(FakeClientSession session)
    {
        LoginGranted grant = Assert.IsType<LoginGranted>(await RedeemAsync(await IssueAsync()));
        var reply = new TaskCompletionSource<WorldDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        _accounts.Post(new RedeemSessionKey(grant.SessionKey, grant.Echo, "alice@example.com", session, reply));
        Assert.IsType<WorldGranted>(await reply.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken));
    }

    private async Task<Guid> IssueAsync()
    {
        var reply = new TaskCompletionSource<Guid>(TaskCreationOptions.RunContinuationsAsynchronously);
        _accounts.Post(new IssueGameToken(s_alice, reply));
        return await reply.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }

    private async Task<LoginDecision> RedeemAsync(Guid token)
    {
        var reply = new TaskCompletionSource<LoginDecision>(TaskCreationOptions.RunContinuationsAsynchronously);
        _accounts.Post(new RedeemGameToken(token, "alice@example.com", reply));
        return await reply.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }
}
