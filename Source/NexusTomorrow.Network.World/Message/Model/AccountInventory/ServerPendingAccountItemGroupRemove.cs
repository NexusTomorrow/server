using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerPendingAccountItemGroupRemove, Direction.ServerToClient)]
public sealed partial class ServerPendingAccountItemGroupRemove
{
    [UInt(64)] public ulong PendingItemGroupId { get; set; } // a bit of a guess
    [UInt(1)] public bool Unused { get; set; }
}
