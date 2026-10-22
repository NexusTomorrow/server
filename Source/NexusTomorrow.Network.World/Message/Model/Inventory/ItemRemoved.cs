using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Removes an item from the client's inventory.
/// </summary>
/// <remarks>
/// The client raises ItemRemoved with the item, its whole stack and <see cref="Reason"/>, or ItemSentToCrate for reason 24
/// (HousingCrate).
/// </remarks>
[WireMessage(GameMessageOpcode.Server0148, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ItemRemoved
{
    /// <summary>
    /// The item, by id.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; set; }

    /// <summary>
    /// Why: an Item.CodeEnumItemUpdateReason value, which the client's events pass on.
    /// </summary>
    [UInt(6)] public byte Reason { get; set; }
}
