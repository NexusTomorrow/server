using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Replaces the pending items, grouped and ungrouped, with these; the client raises AccountPendingItemsUpdate and AccountItemUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server0979, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class AccountPendingItems
{
    /// <summary>
    /// The pending items.
    /// </summary>
    [Array(CountBits = 32)] public List<AccountPendingItem> Items { get; set; } = [];
}
