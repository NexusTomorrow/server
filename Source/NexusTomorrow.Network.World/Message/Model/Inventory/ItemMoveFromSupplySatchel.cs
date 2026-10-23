using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Moves items out of the supply satchel: the request the script call Item:TakeFromSupplySatchel sends, among others.
/// </summary>
[WireMessage(GameMessageOpcode.Client0184, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class ItemMoveFromSupplySatchel
{
    [UInt(14)] public ushort Unknown0 { get; private set; }

    /// <summary>
    /// How many to take: the script call's second argument, or without one all the satchel holds of the item's material; from
    /// the bag window, the count dragged.
    /// </summary>
    [UInt(32)] public uint Count { get; private set; }

    /// <summary>
    /// Where they go: from the script call no location in particular (300, slot 0xFFFFFFFF); from the bag window, the slot they
    /// are dropped on.
    /// </summary>
    public ItemLocation Destination { get; } = new();
}
