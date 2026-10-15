using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerUnitSetChair)]
[WireMessage(GameMessageOpcode.ServerUnitSetChair, Direction.ServerToClient)]
public sealed partial class ServerUnitSetChair : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint UnitIdChair { get; set; }
    [UInt(1)] public bool WaitForUnit { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(UnitIdChair);
        writer.Write(WaitForUnit);
    }
}
