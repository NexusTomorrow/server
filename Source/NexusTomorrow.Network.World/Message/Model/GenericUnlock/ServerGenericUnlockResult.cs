using NexusTomorrow.Game.Static.GenericUnlock;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[WireMessage(GameMessageOpcode.ServerGenericUnlockResult, Direction.ServerToClient)]
public sealed partial class ServerGenericUnlockResult
{
    [UInt(3)] public GenericUnlockResult Result { get; set; }
}
