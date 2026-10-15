using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupMemberAdd)]
[WireMessage(GameMessageOpcode.ServerGroupMemberAdd, Direction.ServerToClient)]
public sealed partial class ServerGroupMemberAdd : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(32)] public uint Unknown0 { get; set; }
    public GroupMember AddedMember { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.Write(Unknown0);
        AddedMember.Write(writer);
    }
}
