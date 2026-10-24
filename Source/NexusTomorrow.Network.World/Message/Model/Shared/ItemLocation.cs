using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// Where an item is among the player's inventories.
/// </summary>
[WireStruct]
public sealed partial class ItemLocation
{
    /// <summary>
    /// Which inventory holds the item, the public interface's EInventoryLocation: 0 the equipped items, 1 the bags. 300,
    /// with a slot of 0xFFFFFFFF, means no location.
    /// </summary>
    [VerifiedMember, UInt(9)] public ushort Location { get; set; }

    /// <summary>
    /// The slot in that inventory.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Slot { get; set; }
}
