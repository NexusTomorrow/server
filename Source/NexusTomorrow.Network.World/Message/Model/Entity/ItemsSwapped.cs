using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Swaps two items among the player's inventories.
/// </summary>
/// <remarks>
/// The client raises PlayerEquippedItemChanged for each equipped slot involved.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0568, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ItemsSwapped
{
    /// <summary>
    /// The first item and its slot.
    /// </summary>
    public ItemPlacement First { get; set; } = new();

    /// <summary>
    /// The second item and its slot.
    /// </summary>
    public ItemPlacement Second { get; set; } = new();
}
