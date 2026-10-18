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
    /// <remarks>
    /// One query reads the account with the suspensions in force, so an email with no account costs the database the same work as one
    /// with an account, and the time an answer takes does not tell which emails have accounts.
    /// </remarks>
    public async Task<AccountCredentials?> FindAsync(string email, CancellationToken cancellationToken)
    {
        await using AccountContext context = _contexts.CreateAccount();
        DateTimeOffset now = _time.GetUtcNow();

        // The email column's collation makes this comparison ignore case.
        Found? found = await context.Accounts
            .AsNoTracking()
            .Where(a => a.Email == email)
            .Select(a => new Found(
                a,
                context.Suspensions
                    .Where(s => s.AccountId == a.Id && s.StartsAt <= now && (s.EndsAt == null || s.EndsAt > now))
                    .Select(s => s.EndsAt)
                    .ToList()))
            .SingleOrDefaultAsync(cancellationToken);
        if (found is null)
            return null;

        AccountRow account = found.Account;
        bool indefinite = found.SuspensionEnds.Contains(null);
        DateTimeOffset? until = indefinite ? null : found.SuspensionEnds.Max();
        return new AccountCredentials(account.Id, account.Email, account.Salt, account.Verifier, until, indefinite);
    }

    private sealed record Found(AccountRow Account, List<DateTimeOffset?> SuspensionEnds);
}
