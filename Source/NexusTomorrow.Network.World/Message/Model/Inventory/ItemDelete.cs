using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Deletes an item: the request the client sends when the player confirms deleting one.
/// </summary>
/// <remarks>
/// The client sends it only for a location that holds an item, never for an item its Item2 row marks undeletable, and not for an
/// equipped item the row forbids deleting while equipped.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0149, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ItemDelete
{
    /// <summary>
    /// Where the item is.
    /// </summary>
    public ItemLocation Location { get; } = new();
}
