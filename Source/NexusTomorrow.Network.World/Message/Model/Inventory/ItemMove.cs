using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Moves an item from one location to another: the request the script call GameLib.SwapBagItems leads to, among others.
/// </summary>
/// <remarks>
/// The client refuses a destination at location 300 or more, at slot 0xFFFFFFFF, or among the equipped items (location 0)
/// above slot 30.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0182, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ItemMove
{
    /// <summary>
    /// Where the item is.
    /// </summary>
    public ItemLocation From { get; } = new();

    /// <summary>
    /// Where it goes.
    /// </summary>
    public ItemLocation To { get; } = new();
}
