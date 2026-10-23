using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Moves an item among the player's inventories.
/// </summary>
/// <remarks>
/// The client raises PlayerEquippedItemChanged for an equipped slot the item leaves or enters.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0569, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ItemMoved
{
    /// <summary>
    /// The item and the slot it moves to.
    /// </summary>
    public ItemPlacement Placement { get; set; } = new();
}
