using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Asks to take an item from the account's inventory; the client sends only an id it holds.
/// </summary>
[WireMessage(GameMessageOpcode.Client0839, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class AccountItemTake
{
    /// <summary>
    /// The item, by <see cref="Shared.AccountItem.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; private set; }
}
