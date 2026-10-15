using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMovementControl)]
[WireMessage(GameMessageOpcode.ServerMovementControl, Direction.ServerToClient)]
public sealed partial class ServerMovementControl : IWritable
{
    [UInt(32)] public uint Ticket { get; set; }
    [UInt(1)] public bool Immediate { get; set; }
    [UInt(32)] public uint UnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Ticket);
        writer.Write(Immediate);
        writer.Write(UnitId);
    }
}
