using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Salvages an item: the request the client sends when the player confirms salvaging one.
/// </summary>
/// <remarks>
/// The client sends it only for an item it judges salvageable.
/// </remarks>
[WireMessage(GameMessageOpcode.Client015E, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ItemSalvage
{
    /// <summary>
    /// Where the item is.
    /// </summary>
    public ItemLocation Location { get; } = new();

    /// <summary>
    /// The item, by id.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; set; }
}
