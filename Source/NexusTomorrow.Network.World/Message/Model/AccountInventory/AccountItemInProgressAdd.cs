using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Adds an account item to the items in progress. While an item in progress has no currency in its AccountItem row, the client
/// counts a CREDD redemption as in progress.
/// </summary>
[WireMessage(GameMessageOpcode.Server096A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountItemInProgressAdd
{
    /// <summary>
    /// The client gives this value no meaning. It discards it.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The item.
    /// </summary>
    public AccountItem Item { get; set; } = new();
}
