using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Adds one item to the account's inventory and marks it new; the client raises AccountInventoryUpdate and AccountItemUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server0975, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountItemAdd
{
    /// <summary>
    /// The item.
    /// </summary>
    public AccountItem Item { get; set; } = new();
}
