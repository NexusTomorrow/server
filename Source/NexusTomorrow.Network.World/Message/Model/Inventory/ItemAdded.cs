using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Adds an item to the client's inventory.
/// </summary>
/// <remarks>
/// Unless the item's location is 4, the client then raises ItemAdded.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0111, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ItemAdded
{
    /// <summary>
    /// The item, with why it was added.
    /// </summary>
    [VerifiedMember] public ItemUpdate Update { get; set; } = new();
}
