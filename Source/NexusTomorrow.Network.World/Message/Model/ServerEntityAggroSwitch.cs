using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEntityAggroSwitch, Direction.ServerToClient)]
public sealed partial class ServerEntityAggroSwitch
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint TargetId { get; set; }
}
