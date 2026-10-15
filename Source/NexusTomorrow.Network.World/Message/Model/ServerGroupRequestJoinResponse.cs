using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupRequestJoinResponse)]
[WireMessage(GameMessageOpcode.ServerGroupRequestJoinResponse, Direction.ServerToClient)]
public sealed partial class ServerGroupRequestJoinResponse : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }

    public GroupMember MemberInfo { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        MemberInfo.Write(writer);
    }
}
