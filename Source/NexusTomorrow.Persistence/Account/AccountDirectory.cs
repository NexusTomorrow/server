using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Account;

/// <summary>
/// Reads accounts straight from the database, judging suspensions by the injected clock.
/// </summary>
public sealed class AccountDirectory : IAccountDirectory
{
    private readonly PersistenceContexts _contexts;
    private readonly TimeProvider _time;

    /// <summary>
    /// Create a directory reading through <paramref name="contexts"/>, judging suspensions by <paramref name="time"/>.
    /// </summary>
    public AccountDirectory(PersistenceContexts contexts, TimeProvider time)
    {
        _contexts = contexts;
        _time = time;
    }

    /// <inheritdoc/>
    public async Task<AccountCredentials?> FindAsync(string email, CancellationToken cancellationToken)
    {
        await using AccountContext context = _contexts.CreateAccount();
        // The email column's collation makes this comparison ignore case.
        AccountRow? account = await context.Accounts.AsNoTracking().SingleOrDefaultAsync(a => a.Email == email, cancellationToken);
        if (account is null)
            return null;

        DateTimeOffset now = _time.GetUtcNow();
        List<DateTimeOffset?> ends = await context.Suspensions
            .AsNoTracking()
            .Where(s => s.AccountId == account.Id && s.StartsAt <= now && (s.EndsAt == null || s.EndsAt > now))
            .Select(s => s.EndsAt)
            .ToListAsync(cancellationToken);
        bool indefinite = ends.Contains(null);
        DateTimeOffset? until = indefinite ? null : ends.Max();
        return new AccountCredentials(account.Id, account.Email, account.Salt, account.Verifier, until, indefinite);
    }
}
