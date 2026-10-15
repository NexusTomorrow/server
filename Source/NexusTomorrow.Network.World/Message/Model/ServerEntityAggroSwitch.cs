using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityAggroSwitch)]
[WireMessage(GameMessageOpcode.ServerEntityAggroSwitch, Direction.ServerToClient)]
public sealed partial class ServerEntityAggroSwitch : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint TargetId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(TargetId);
    }
}
