using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Moves an item within the guild bank: the request GuildTypeLib's EndBankItemTransfer sends when both slots are in the bank.
/// </summary>
/// <remarks>
/// GuildTypeLib's BeginBankItemTransfer picks the item and the count.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0471, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GuildBankMoveItem
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; private set; } = new RealmScopedId();

    /// <summary>
    /// The item to move, by id.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; private set; }

    /// <summary>
    /// The item in the destination slot, by id; 0 for an empty slot.
    /// </summary>
    [UInt(64)] public ulong DestinationItemId { get; private set; }

    /// <summary>
    /// Where the item is.
    /// </summary>
    public ItemLocation Source { get; private set; } = new ItemLocation();

    /// <summary>
    /// Where it goes.
    /// </summary>
    public ItemLocation Destination { get; private set; } = new ItemLocation();

    /// <summary>
    /// How many to move.
    /// </summary>
    [UInt(32)] public uint Quantity { get; private set; }
}
