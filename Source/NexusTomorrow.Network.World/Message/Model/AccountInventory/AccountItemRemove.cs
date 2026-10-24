using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Removes an item from the account's inventory; the client raises AccountInventoryUpdate and AccountItemUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server097C, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountItemRemove
{
    /// <summary>
    /// The item, by <see cref="Shared.AccountItem.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }
}
