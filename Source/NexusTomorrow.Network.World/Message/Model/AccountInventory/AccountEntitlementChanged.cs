using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Sets one of the account's entitlements; the client raises AccountEntitlementUpdate, and prints a chat line when the count rose.
/// </summary>
[WireMessage(GameMessageOpcode.Server0973, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountEntitlementChanged
{
    /// <summary>
    /// The entitlement, an Entitlement table id.
    /// </summary>
    [UInt(32)] public uint EntitlementId { get; set; }

    /// <summary>
    /// How many of the entitlement the account now holds.
    /// </summary>
    [UInt(32)] public uint Count { get; set; }
}
