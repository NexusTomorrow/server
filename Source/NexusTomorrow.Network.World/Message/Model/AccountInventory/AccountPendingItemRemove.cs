using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Removes an ungrouped pending item.
/// </summary>
[WireMessage(GameMessageOpcode.Server097E, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountPendingItemRemove
{
    /// <summary>
    /// The item, by <see cref="Shared.AccountPendingItem.Id"/>; the client also clears its new mark.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It discards it.
    /// </summary>
    [UInt(1)] public bool Unknown1 { get; set; }
}
