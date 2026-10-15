using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[WireMessage(GameMessageOpcode.ServerGenericUnlockAccount, Direction.ServerToClient)]
public sealed partial class ServerGenericUnlock
{
    [UInt(14)] public ushort GenericUnlockEntryId { get; set; }
}
