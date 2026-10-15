using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildMemberChange)]
[WireMessage(GameMessageOpcode.ServerGuildMemberChange, Direction.ServerToClient)]
public sealed partial class ServerGuildMemberChange : IWritable
{
    public Identity GuildIdentity { get; set; }
    public GuildMember GuildMember { get; set; }
    [UInt(16)] public ushort MemberCount { get; set; }
    [UInt(16)] public ushort OnlineMemberCount { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        GuildMember.Write(writer);
        writer.Write(MemberCount);
        writer.Write(OnlineMemberCount);
    }
}
