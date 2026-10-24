namespace NexusTomorrow.Owners.Account;

/// <summary>
/// The most of each entitlement the client counts: its Entitlement row's maxCount. In the world the client caps a character's count of
/// an entitlement, character plus account, at that number.
/// </summary>
public interface IEntitlementCaps
{
    /// <summary>
    /// The entitlement's maxCount, 0 when its row sets no cap; null when no row names it.
    /// </summary>
    uint? MaxCountOf(uint entitlementId);
}
