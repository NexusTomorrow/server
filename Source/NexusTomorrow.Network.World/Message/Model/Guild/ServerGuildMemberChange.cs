using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildMemberChange, Direction.ServerToClient)]
public sealed partial class ServerGuildMemberChange
{
    public Identity GuildIdentity { get; set; }
    public GuildMember GuildMember { get; set; }
    [UInt(16)] public ushort MemberCount { get; set; }
    [UInt(16)] public ushort OnlineMemberCount { get; set; }
}
