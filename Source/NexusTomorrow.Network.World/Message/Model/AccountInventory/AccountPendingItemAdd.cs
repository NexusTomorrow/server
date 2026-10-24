using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Adds one pending item and marks it new; the client raises AccountPendingItemsUpdate and AccountItemUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server0976, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class AccountPendingItemAdd
{
    /// <summary>
    /// The item.
    /// </summary>
    public AccountPendingItem Item { get; set; } = new();
}
