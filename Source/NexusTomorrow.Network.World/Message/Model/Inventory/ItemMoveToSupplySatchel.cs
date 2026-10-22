using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Moves an item into the supply satchel: the request the script call Item:MoveToSupplySatchel sends, among others.
/// </summary>
[WireMessage(GameMessageOpcode.Client012A, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ItemMoveToSupplySatchel
{
    /// <summary>
    /// The item, by id.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; private set; }

    /// <summary>
    /// How many to move: the script call's second argument, or the whole stack without one.
    /// </summary>
    [UInt(32)] public uint Count { get; private set; }
}
