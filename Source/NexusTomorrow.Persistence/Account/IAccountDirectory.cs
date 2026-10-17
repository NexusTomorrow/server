namespace NexusTomorrow.Persistence.Account;

/// <summary>
/// Reads accounts for the STS. The gateway is not an owner, so it reads directly instead of through the write-behind queue.
/// </summary>
public interface IAccountDirectory
{
    /// <summary>
    /// The account whose email matches <paramref name="email"/> without regard to case, or null.
    /// </summary>
    Task<AccountCredentials?> FindAsync(string email, CancellationToken cancellationToken);
}
