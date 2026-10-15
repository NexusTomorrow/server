using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerRecruitmentGuildDetails, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildDetails
{
    public Identity GuildIdentity { get; set; }
    [WideString] public string Description { get; set; }
    [Float32] public float GuildCreationDaysAgo { get; set; }
    [UInt(1)] public bool HasTax { get; set; }
    [UInt(32)] public uint RecruitmentMinimumLevel { get; set; }
    public RecruitmentDemands Demands { get; set; }
}
