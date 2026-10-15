using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildMemberChange)]
public class ServerGuildMemberChange : IWritable
{
    public Identity GuildIdentity { get; set; }
    public GuildMember GuildMember { get; set; }
    public ushort MemberCount { get; set; }
    public ushort OnlineMemberCount { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        GuildMember.Write(writer);
        writer.Write(MemberCount);
        writer.Write(OnlineMemberCount);
    }
}
