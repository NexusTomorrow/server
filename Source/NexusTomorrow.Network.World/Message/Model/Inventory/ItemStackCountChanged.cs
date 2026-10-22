using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Sets an item's stack count.
/// </summary>
/// <remarks>
/// The client raises ItemAdded when the count rose and ItemRemoved (ItemSentToCrate for reason 24) otherwise, each with the
/// difference and <see cref="Reason"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server017F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ItemStackCountChanged
{
    /// <summary>
    /// The item, by id.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; set; }

    /// <summary>
    /// The item's new stack count.
    /// </summary>
    [UInt(32)] public uint StackCount { get; set; }

    /// <summary>
    /// Why: an Item.CodeEnumItemUpdateReason value, which the client's events pass on.
    /// </summary>
    [UInt(6)] public byte Reason { get; set; }
}
