using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Persistence.Ops;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Persistence.Tests;

public sealed class AccountDirectoryTests
{
    [Fact]
    public async Task AnAccountIsFoundByEmailInAnyCaseWithItsSuspension()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 10, 16, 0, 0, 0, TimeSpan.Zero));
        await using (AccountContext context = database.Contexts.CreateAccount())
        {
            var account = new AccountRow { Email = "alice@example.com", Salt = [1, 2], Verifier = [3, 4], CreatedAt = time.GetUtcNow() };
            context.Accounts.Add(account);
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            context.Suspensions.Add(new SuspensionRow { AccountId = account.Id, StartsAt = time.GetUtcNow(), EndsAt = time.GetUtcNow().AddDays(2), Reason = "test", CreatedBy = "test" });
            context.Suspensions.Add(new SuspensionRow { AccountId = account.Id, StartsAt = time.GetUtcNow().AddDays(-9), EndsAt = time.GetUtcNow().AddDays(-8), Reason = "old", CreatedBy = "test" });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        AccountCredentials? found = await new AccountDirectory(database.Contexts, time).FindAsync("Alice@Example.COM", TestContext.Current.CancellationToken);
        Assert.NotNull(found);
        Assert.Equal([1, 2], found.Salt);
        Assert.Equal(time.GetUtcNow().AddDays(2), found.SuspendedUntil);
        Assert.False(found.SuspendedIndefinitely);
    }

    [Fact]
    public async Task AnIndefiniteSuspensionWinsAndOneNotYetStartedIsNotInForce()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 10, 16, 0, 0, 0, TimeSpan.Zero));
        DateTimeOffset now = time.GetUtcNow();
        long alice = await AddAccountAsync(database, "alice@example.com", now, (now.AddDays(-1), now.AddDays(5)), (now.AddDays(-1), null));
        long bob = await AddAccountAsync(database, "bob@example.com", now, (now.AddDays(1), null));
        var directory = new AccountDirectory(database.Contexts, time);

        AccountCredentials? indefinite = await directory.FindAsync("alice@example.com", TestContext.Current.CancellationToken);
        Assert.Equal(alice, indefinite!.Id);
        Assert.True(indefinite.SuspendedIndefinitely);
        Assert.Null(indefinite.SuspendedUntil);

        AccountCredentials? later = await directory.FindAsync("bob@example.com", TestContext.Current.CancellationToken);
        Assert.Equal(bob, later!.Id);
        Assert.False(later.SuspendedIndefinitely);
        Assert.Null(later.SuspendedUntil);
    }

    [Fact]
    public async Task AnUnknownEmailIsNotFound()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        Assert.Null(await new AccountDirectory(database.Contexts, TimeProvider.System).FindAsync("nobody@example.com", TestContext.Current.CancellationToken));
    }

    // An unknown email must cost the database what a known one does, or the time an answer takes tells which emails have accounts.
    [Fact]
    public async Task AKnownAndAnUnknownEmailRunTheSameDatabaseCommands()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        var time = new FakeTimeProvider(new DateTimeOffset(2026, 10, 16, 0, 0, 0, TimeSpan.Zero));
        DateTimeOffset now = time.GetUtcNow();
        await AddAccountAsync(database, "alice@example.com", now, (now.AddDays(-1), now.AddDays(1)));
        var commands = new CommandRecorder();
        var directory = new AccountDirectory(Recorded(database.ConnectionString, commands), time);

        Assert.NotNull(await directory.FindAsync("alice@example.com", TestContext.Current.CancellationToken));
        string[] known = commands.Take();
        Assert.Null(await directory.FindAsync("nobody@example.com", TestContext.Current.CancellationToken));
        string[] unknown = commands.Take();

        Assert.Single(known);
        Assert.Equal(known, unknown);
    }

    private static async Task<long> AddAccountAsync(
        PostgresTestDatabase database, string email, DateTimeOffset now, params (DateTimeOffset StartsAt, DateTimeOffset? EndsAt)[] suspensions)
    {
        await using AccountContext context = database.Contexts.CreateAccount();
        var account = new AccountRow { Email = email, Salt = [1, 2], Verifier = [3, 4], CreatedAt = now };
        context.Accounts.Add(account);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        foreach ((DateTimeOffset startsAt, DateTimeOffset? endsAt) in suspensions)
        {
            context.Suspensions.Add(
                new SuspensionRow { AccountId = account.Id, StartsAt = startsAt, EndsAt = endsAt, Reason = "test", CreatedBy = "test" });
        }
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        return account.Id;
    }

    // The production contexts' options with the recorder added, so only the commands of the contexts these factories make are recorded.
    private static PersistenceContexts Recorded(string connectionString, CommandRecorder recorder) => new(
        new PooledDbContextFactory<AccountContext>(new DbContextOptionsBuilder<AccountContext>(
            SchemaConventions.Options<AccountContext>(connectionString, AccountContext.Schema)).AddInterceptors(recorder).Options),
        new PooledDbContextFactory<RealmContext>(SchemaConventions.Options<RealmContext>(connectionString, RealmContext.Schema)),
        new PooledDbContextFactory<OpsContext>(SchemaConventions.Options<OpsContext>(connectionString, OpsContext.Schema)));

    // The text of every command EF Core makes ready to run, in order.
    private sealed class CommandRecorder : DbCommandInterceptor
    {
        private readonly Lock _gate = new();
        private readonly List<string> _commands = [];

        public string[] Take()
        {
            lock (_gate)
            {
                string[] taken = [.. _commands];
                _commands.Clear();
                return taken;
            }
        }

        public override DbCommand CommandInitialized(CommandEndEventData eventData, DbCommand result)
        {
            lock (_gate)
                _commands.Add(result.CommandText);
            return result;
        }
    }
}
