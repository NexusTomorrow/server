using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMovementControl, Direction.ServerToClient)]
public sealed partial class ServerMovementControl
{
    [UInt(32)] public uint Ticket { get; set; }
    [UInt(1)] public bool Immediate { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
}
