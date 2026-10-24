using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Removes a pending item group.
/// </summary>
[WireMessage(GameMessageOpcode.Server097D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountPendingItemGroupRemove
{
    /// <summary>
    /// The group, by <see cref="Shared.AccountPendingItem.GroupName"/>.
    /// </summary>
    [WideString] public string GroupName { get; set; } = "";

    /// <summary>
    /// The client gives this value no meaning. It discards it.
    /// </summary>
    [UInt(1)] public bool Unknown1 { get; set; }
}
