using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Sets an item's durability.
/// </summary>
/// <remarks>
/// The client then raises ItemDurabilityUpdate with the item and its previous durability.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01A7, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ItemDurabilityUpdate
{
    /// <summary>
    /// The item, by id.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; set; }

    /// <summary>
    /// The item's new durability, kept as a float; a new item starts at 1.0.
    /// </summary>
    [Float32] public float Durability { get; set; }
}
