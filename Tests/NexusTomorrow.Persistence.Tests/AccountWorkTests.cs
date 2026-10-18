using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Persistence.Ops;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Persistence.Tests;

public sealed class AccountWorkTests
{
    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(30);

    private readonly FakeTimeProvider _time = new(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));

    [Fact]
    public async Task ACreatedAccountIsFoundByTheDirectory()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        Assert.Equal(WorkOutcome.Committed, (await CreateAsync(queue, "Alice@Example.com")).Outcome);

        AccountCredentials? found = await new AccountDirectory(database.Contexts, _time).FindAsync("alice@example.com", TestContext.Current.CancellationToken);
        Assert.NotNull(found);
        Assert.Equal("Alice@Example.com", found.Email);
        Assert.Equal([1, 2], found.Salt);
        Assert.Equal([3, 4], found.Verifier);
        Assert.Null(found.SuspendedUntil);
        Assert.False(found.SuspendedIndefinitely);
    }

    [Fact]
    public async Task ACreateWithATakenEmailInAnyCaseIsRefusedByItsIndex()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        Assert.Equal(WorkOutcome.Committed, (await CreateAsync(queue, "alice@example.com")).Outcome);

        WorkResult refused = await CreateAsync(queue, "ALICE@EXAMPLE.COM");
        Assert.Equal(WorkOutcome.Refused, refused.Outcome);
        Assert.Equal("ix_account_email", refused.Constraint);
    }

    [Fact]
    public async Task SuspendingAnUnknownEmailWritesNothing()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        long? id = await SuspendAsync(queue, "nobody@example.com", endsAt: null);

        Assert.Null(id);
        await using AccountContext context = database.Contexts.CreateAccount();
        Assert.Empty(await context.Suspensions.ToListAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task LiftingAnUnknownEmailWritesNothing()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        Assert.Null(await LiftAsync(queue, "nobody@example.com"));
    }

    [Fact]
    public async Task ASuspensionAndItsLiftShowInTheDirectory()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        var directory = new AccountDirectory(database.Contexts, _time);
        await CreateAsync(queue, "alice@example.com");

        long? suspended = await SuspendAsync(queue, "Alice@Example.com", endsAt: null);
        Assert.NotNull(suspended);
        Assert.True((await directory.FindAsync("alice@example.com", TestContext.Current.CancellationToken))!.SuspendedIndefinitely);

        _time.Advance(TimeSpan.FromHours(1));
        Assert.Equal(suspended, await LiftAsync(queue, "alice@example.com"));
        AccountCredentials lifted = (await directory.FindAsync("alice@example.com", TestContext.Current.CancellationToken))!;
        Assert.False(lifted.SuspendedIndefinitely);
        Assert.Null(lifted.SuspendedUntil);
    }

    [Fact]
    public async Task ATimedSuspensionIsLiftedWithTheIndefiniteOne()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        var directory = new AccountDirectory(database.Contexts, _time);
        await CreateAsync(queue, "alice@example.com");
        await SuspendAsync(queue, "alice@example.com", _time.GetUtcNow().AddDays(2));
        await SuspendAsync(queue, "alice@example.com", endsAt: null);

        await LiftAsync(queue, "alice@example.com");
        AccountCredentials lifted = (await directory.FindAsync("alice@example.com", TestContext.Current.CancellationToken))!;
        Assert.False(lifted.SuspendedIndefinitely);
        Assert.Null(lifted.SuspendedUntil);
    }

    [Fact]
    public async Task AnAuditRowIsWritten()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using WriteBehindQueue queue = Databases.Queue(database);
        queue.Enqueue(new AuditWork(new AuditRow
        {
            At = _time.GetUtcNow(),
            Actor = "console",
            FrontEnd = "Cli",
            Action = "account create",
            Target = "alice@example.com",
            Details = "email=alice@example.com",
            RemoteAddress = null,
            Outcome = "Done",
        }));
        await queue.DrainAsync(TestContext.Current.CancellationToken).WaitAsync(s_bound, TestContext.Current.CancellationToken);

        await using OpsContext context = database.Contexts.CreateOps();
        AuditRow stored = Assert.Single(await context.Audit.ToListAsync(TestContext.Current.CancellationToken));
        Assert.Equal("account create", stored.Action);
        Assert.Equal("alice@example.com", stored.Target);
        Assert.Equal("Done", stored.Outcome);
        Assert.Null(stored.RemoteAddress);
        Assert.Equal(_time.GetUtcNow(), stored.At);
    }

    private async Task<WorkResult> CreateAsync(WriteBehindQueue queue, string email)
    {
        var done = new TaskCompletionSource<WorkResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new CreateAccountWork(email, [1, 2], [3, 4], _time.GetUtcNow(), result => done.SetResult(result)));
        return await done.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }

    private async Task<long?> SuspendAsync(WriteBehindQueue queue, string email, DateTimeOffset? endsAt)
    {
        var done = new TaskCompletionSource<long?>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new SuspendAccountWork(email, _time.GetUtcNow(), endsAt, "test", "test", id => done.SetResult(id)));
        return await done.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }

    private async Task<long?> LiftAsync(WriteBehindQueue queue, string email)
    {
        var done = new TaskCompletionSource<long?>(TaskCreationOptions.RunContinuationsAsynchronously);
        queue.Enqueue(new LiftSuspensionWork(email, _time.GetUtcNow(), id => done.SetResult(id)));
        return await done.Task.WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }
}
