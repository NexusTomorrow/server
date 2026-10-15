using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityGroupAssociation)]
[WireMessage(GameMessageOpcode.ServerEntityGroupAssociation, Direction.ServerToClient)]
public sealed partial class ServerEntityGroupAssociation : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(64)] public ulong GroupId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(GroupId);
    }
}
