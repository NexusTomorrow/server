using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Adds items to the account's inventory; the client raises AccountInventoryUpdate and AccountItemUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server096D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountItems
{
    /// <summary>
    /// The items.
    /// </summary>
    [Array(CountBits = 32)] public List<AccountItem> Items { get; set; } = [];
}
