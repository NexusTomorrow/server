using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Sets each listed entitlement; the client raises AccountEntitlementUpdate for each.
/// </summary>
[WireMessage(GameMessageOpcode.Server0968, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountEntitlements
{
    /// <summary>
    /// One account entitlement and its count.
    /// </summary>
    [WireStruct]
    public sealed partial class AccountEntitlement
    {
        /// <summary>
        /// The entitlement, an Entitlement table id.
        /// </summary>
        [UInt(32)] public uint EntitlementId { get; set; }

        /// <summary>
        /// How many of the entitlement the account holds.
        /// </summary>
        [UInt(32)] public uint Count { get; set; }
    }

    /// <summary>
    /// The entitlements.
    /// </summary>
    [Array(CountBits = 32)] public List<AccountEntitlement> Entitlements { get; set; } = [];
}
