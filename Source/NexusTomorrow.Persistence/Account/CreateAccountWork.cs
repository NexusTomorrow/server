namespace NexusTomorrow.Persistence.Account;

/// <summary>
/// Writes a new account with its SRP6 salt and verifier. A taken email, in any case, ends as <see cref="WorkOutcome.Refused"/> by
/// the unique index on <c>account.account.email</c>.
/// </summary>
public sealed class CreateAccountWork : PersistenceWork
{
    private readonly DateTimeOffset _at;
    private readonly Action<WorkResult> _done;

    /// <summary>
    /// Create work that writes the account for <paramref name="email"/> at <paramref name="at"/> and reports its outcome to
    /// <paramref name="done"/>.
    /// </summary>
    public CreateAccountWork(string email, byte[] salt, byte[] verifier, DateTimeOffset at, Action<WorkResult> done)
        : base(PersistKey.Email(email))
    {
        Email = email;
        Salt = salt;
        Verifier = verifier;
        _at = at;
        _done = done;
    }

    /// <summary>
    /// The email the account logs in with, as the operator typed it.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// The salt the verifier was made with.
    /// </summary>
    public byte[] Salt { get; }

    /// <summary>
    /// The verifier the account's password makes with <see cref="Salt"/>.
    /// </summary>
    internal byte[] Verifier { get; }

    /// <inheritdoc/>
    public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
    {
        // A fresh context each attempt, so a retry never carries a failed attempt's tracked row.
        await using AccountContext context = contexts.CreateAccount();
        context.Accounts.Add(new AccountRow { Email = Email, Salt = Salt, Verifier = Verifier, CreatedAt = _at });
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override void Complete(WorkResult result) => _done(result);
}
