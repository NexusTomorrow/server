using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerRecruitmentGuildDemands, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildDemands
{
    public Identity GuildIdentity { get; set; }
    public RecruitmentDemands Demands { get; set; }
}

