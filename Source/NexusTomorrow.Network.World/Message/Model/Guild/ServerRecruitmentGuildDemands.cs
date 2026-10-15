using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerRecruitmentGuildDemands)]
[WireMessage(GameMessageOpcode.ServerRecruitmentGuildDemands, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildDemands : IWritable
{
    public Identity GuildIdentity { get; set; }
    public RecruitmentDemands Demands { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        Demands.Write(writer);
    }
}

