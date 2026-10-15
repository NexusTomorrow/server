using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerUnitInUse)]
[WireMessage(GameMessageOpcode.ServerUnitInUse, Direction.ServerToClient)]
public sealed partial class ServerUnitInUse : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(1)] public bool InUse { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(InUse);
    }
}
