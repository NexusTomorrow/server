using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupInviteReceived)]
[WireMessage(GameMessageOpcode.ServerGroupInviteReceived, Direction.ServerToClient)]
public sealed partial class ServerGroupInviteReceived : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(32)] public uint LeaderIndex { get; set; }
    [UInt(32)] public uint InviterIndex { get; set; }

    [Array(CountBits = 32)] public List<GroupCharacter> Members = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.Write(LeaderIndex);
        writer.Write(InviterIndex);

        writer.Write(Members.Count);
        Members.ForEach(x => x.Write(writer));
    }
}
