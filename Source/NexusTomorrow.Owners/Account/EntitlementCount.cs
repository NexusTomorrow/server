namespace NexusTomorrow.Owners.Account;

/// <summary>
/// One entitlement an account holds and how many of it.
/// </summary>
public readonly record struct EntitlementCount(uint Id, uint Count);
