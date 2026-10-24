using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Removes an account item from the items in progress.
/// </summary>
[WireMessage(GameMessageOpcode.Server096C, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountItemInProgressRemove
{
    /// <summary>
    /// The client gives this value no meaning. It discards it.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The item, by <see cref="Shared.AccountItem.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }
}
