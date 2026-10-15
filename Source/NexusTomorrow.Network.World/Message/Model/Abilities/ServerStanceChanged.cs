using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ServerStanceChanged, Direction.ServerToClient)]
public sealed partial class ServerStanceChanged
{
    [UInt(2)] public byte InnateIndex { get; set; }
}
