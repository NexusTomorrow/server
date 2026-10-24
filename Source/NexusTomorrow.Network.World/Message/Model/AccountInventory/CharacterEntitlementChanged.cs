using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Sets one of the character's entitlements; the client raises CharacterEntitlementUpdate, and prints a chat line when the count
/// rose.
/// </summary>
[WireMessage(GameMessageOpcode.Server0918, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CharacterEntitlementChanged
{
    /// <summary>
    /// The entitlement, an Entitlement table id.
    /// </summary>
    [UInt(14)] public ushort EntitlementId { get; set; }

    /// <summary>
    /// How many of the entitlement the character now holds.
    /// </summary>
    [UInt(32)] public uint Count { get; set; }
}
