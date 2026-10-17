using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Persistence.Account;
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
    public async Task AnUnknownEmailIsNotFound()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        Assert.Null(await new AccountDirectory(database.Contexts, TimeProvider.System).FindAsync("nobody@example.com", TestContext.Current.CancellationToken));
    }
}
