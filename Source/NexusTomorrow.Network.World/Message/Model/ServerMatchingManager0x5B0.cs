using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingManager0x5B0, Direction.ServerToClient)]
public sealed partial class ServerMatchingManager0x5B0
{
    [UInt(1)] public bool Unknown { get; set; } // possibly indicates InMatchingGameGroup
}
