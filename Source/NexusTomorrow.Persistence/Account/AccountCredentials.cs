namespace NexusTomorrow.Persistence.Account;

/// <summary>
/// What the STS needs to run a login: the account, its SRP6 salt and verifier, and whether a suspension is in force now.
/// </summary>
public sealed record AccountCredentials(long Id, string Email, byte[] Salt, byte[] Verifier, DateTimeOffset? SuspendedUntil, bool SuspendedIndefinitely);
