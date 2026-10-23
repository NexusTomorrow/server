using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// An item and a slot among the player's inventories.
/// </summary>
[WireStruct]
public sealed partial class ItemPlacement
{
    /// <summary>
    /// The item, by id.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; set; }

    /// <summary>
    /// The slot: the inventory in bits 8 to 15, as <see cref="ItemLocation.Location"/> holds it, and the slot within it in
    /// bits 0 to 7.
    /// </summary>
    [UInt(64)] public ulong Location { get; set; }
}
