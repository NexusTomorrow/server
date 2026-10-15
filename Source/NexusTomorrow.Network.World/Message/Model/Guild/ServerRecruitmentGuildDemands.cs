using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerRecruitmentGuildDemands)]
public class ServerRecruitmentGuildDemands : IWritable
{
    public Identity GuildIdentity { get; set; }
    public RecruitmentDemands Demands { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        Demands.Write(writer);
    }
}

