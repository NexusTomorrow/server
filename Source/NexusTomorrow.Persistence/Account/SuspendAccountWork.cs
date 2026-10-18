using Microsoft.EntityFrameworkCore;

namespace NexusTomorrow.Persistence.Account;

/// <summary>
/// Adds a suspension to the account with an email, in any case. An email with no account writes nothing and reports a null id.
/// </summary>
public sealed class SuspendAccountWork : PersistenceWork
{
    private readonly DateTimeOffset _startsAt;
    private readonly string _by;
    private readonly Action<long?> _done;

    /// <summary>
    /// Create work that suspends the account with <paramref name="email"/> from <paramref name="startsAt"/> until
    /// <paramref name="endsAt"/> (null: until lifted) and reports the account's id, or null when there is none, to
    /// <paramref name="done"/>.
    /// </summary>
    public SuspendAccountWork(string email, DateTimeOffset startsAt, DateTimeOffset? endsAt, string reason, string by, Action<long?> done)
        : base(PersistKey.Email(email))
    {
        Email = email;
        _startsAt = startsAt;
        EndsAt = endsAt;
        Reason = reason;
        _by = by;
        _done = done;
    }

    /// <summary>
    /// The email of the account to suspend.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// When the suspension ends; null until lifted.
    /// </summary>
    public DateTimeOffset? EndsAt { get; }

    /// <summary>
    /// The reason stored with the suspension.
    /// </summary>
    public string Reason { get; }

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
            context.Suspensions.Add(new SuspensionRow { AccountId = accountId, StartsAt = _startsAt, EndsAt = EndsAt, Reason = Reason, CreatedBy = _by });
            await context.SaveChangesAsync(cancellationToken);
        }

        AccountId = id;
    }

    /// <inheritdoc/>
    public override void Complete(WorkResult result) => _done(AccountId);
}
