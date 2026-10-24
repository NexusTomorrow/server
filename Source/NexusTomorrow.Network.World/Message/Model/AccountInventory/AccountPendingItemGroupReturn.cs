using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Asks to return a pending item group.
/// </summary>
[WireMessage(GameMessageOpcode.Client07C6, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class AccountPendingItemGroupReturn
{
    /// <summary>
    /// The group, by <see cref="Shared.AccountPendingItem.GroupName"/>.
    /// </summary>
    [WideString] public string GroupName { get; private set; } = "";
}
