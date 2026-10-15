using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupRequestJoinResult)]
[WireMessage(GameMessageOpcode.ServerGroupRequestJoinResult, Direction.ServerToClient)]
public sealed partial class ServerGroupRequestJoinResult : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }

    [WideString] public string Name { get; set; }

    [UInt(5)] public GroupInviteResult Result { get; set; }

    [UInt(1)] public bool IsJoin { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.WriteStringWide(Name);
        writer.Write(Result, 5u);
        writer.Write(IsJoin);
    }
}
