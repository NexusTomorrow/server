using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEntityDestroy, Direction.ServerToClient)]
public sealed partial class ServerEntityDestroy
{
    [UInt(32)] public uint Guid { get; set; }
    [UInt(1)] public bool Unknown0 { get; set; }
}
