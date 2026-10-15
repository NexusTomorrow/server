using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[WireMessage(GameMessageOpcode.ServerGenericUnlockAccountList, Direction.ServerToClient)]
public sealed partial class ServerGenericUnlockAccountList
{
    [Array(CountBits = 32), UInt(32)] public List<uint> GenericUnlockEntryIds { get; set; } = [];
}
