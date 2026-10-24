using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Adds account items to the items in progress, as <see cref="AccountItemInProgressAdd"/> adds one.
/// </summary>
[WireMessage(GameMessageOpcode.Server096B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountItemsInProgress
{
    /// <summary>
    /// The client gives this value no meaning. It discards it.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The items.
    /// </summary>
    [Array(CountBits = 32)] public List<AccountItem> Items { get; set; } = [];
}
