using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Account;

/// <summary>
/// Ends every suspension in force for the account with an email, in any case. An email with no account writes nothing and reports a
/// null id.
/// </summary>
public sealed class LiftSuspensionWork : PersistenceWork
{
    private readonly DateTimeOffset _at;
    private readonly Action<long?> _done;

    /// <summary>
    /// Create work that ends the suspensions in force at <paramref name="at"/> for the account with <paramref name="email"/> and
    /// reports the account's id, or null when there is none, to <paramref name="done"/>.
    /// </summary>
    public LiftSuspensionWork(string email, DateTimeOffset at, Action<long?> done)
        : base(PersistKey.Email(email))
    {
        Email = email;
        _at = at;
        _done = done;
    }

    /// <summary>
    /// The email of the account to lift.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// The id of the account found by the last successful <see cref="ApplyAsync"/>; null when none has that email.
    /// </summary>
    internal long? AccountId { get; set; }

    /// <inheritdoc/>
    public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
    {
        await using AccountContext context = contexts.CreateAccount();
        // The email column's collation makes this comparison ignore case.
        long? id = await context.Accounts.Where(a => a.Email == Email).Select(a => (long?)a.Id).SingleOrDefaultAsync(cancellationToken);
        if (id is { } accountId)
        {
            // Repeatable: a second run finds no suspension still in force.
            await context.Suspensions
                .Where(s => s.AccountId == accountId && s.StartsAt <= _at && (s.EndsAt == null || s.EndsAt > _at))
                .ExecuteUpdateAsync(set => set.SetProperty(s => s.EndsAt, (DateTimeOffset?)_at), cancellationToken);
        }

        AccountId = id;
    }

    /// <inheritdoc/>
    public override void Complete(WorkResult result) => _done(AccountId);
}
