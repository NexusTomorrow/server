using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Moves an item between the guild bank and the player's bags: the request GuildTypeLib's MoveBankItemToInventory and
/// EndBankItemTransfer send.
/// </summary>
[WireMessage(GameMessageOpcode.Client0473, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class GuildBankTransferItem
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; private set; } = new RealmScopedId();

    /// <summary>
    /// The item on the bank side, by id: the one taken out, or the one in the destination slot (0 when it is empty).
    /// </summary>
    [UInt(64)] public ulong BankItemId { get; private set; }

    [UInt(64)] public ulong Unknown2 { get; private set; }
    [UInt(18)] public uint Unknown3 { get; private set; }

    /// <summary>
    /// Where the item goes.
    /// </summary>
    public ItemLocation Destination { get; private set; } = new ItemLocation();

    /// <summary>
    /// Where the item is.
    /// </summary>
    public ItemLocation Source { get; private set; } = new ItemLocation();

    [UInt(1)] public bool Unknown6 { get; private set; }

    /// <summary>
    /// How many to move.
    /// </summary>
    [UInt(32)] public uint Quantity { get; private set; }
}
