namespace NexusTomorrow.Owners.Account;

/// <summary>
/// One entitlement an account holds by default (<see cref="AccountDefaultsOptions.Entitlements"/>).
/// </summary>
public sealed class EntitlementGrant
{
    /// <summary>
    /// The entitlement, an Entitlement table id.
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// How many of it the account holds; unset, the Entitlement row's maxCount, the most the client counts.
    /// </summary>
    public uint? Count { get; set; }
}
