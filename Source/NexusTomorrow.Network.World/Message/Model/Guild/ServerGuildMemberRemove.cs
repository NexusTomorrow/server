using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildMemberRemove)]
[WireMessage(GameMessageOpcode.ServerGuildMemberRemove, Direction.ServerToClient)]
public sealed partial class ServerGuildMemberRemove : IWritable
{
    public Identity GuildIdentity { get; set; }
    public Identity MemberToRemove { get; set; }
    [UInt(16)] public ushort MemberCount { get; set; }
    [UInt(16)] public ushort OnlineMemberCount { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        MemberToRemove.Write(writer);
        writer.Write(MemberCount);
        writer.Write(OnlineMemberCount);
    }
}
