using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerRecruitmentGuildDetails)]
[WireMessage(GameMessageOpcode.ServerRecruitmentGuildDetails, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildDetails : IWritable
{
    public Identity GuildIdentity { get; set; }
    [WideString] public string Description { get; set; }
    [Float32] public float GuildCreationDaysAgo { get; set; }
    [UInt(1)] public bool HasTax { get; set; }
    [UInt(32)] public uint RecruitmentMinimumLevel { get; set; }
    public RecruitmentDemands Demands { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.WriteStringWide(Description);
        writer.Write(GuildCreationDaysAgo);
        writer.Write(HasTax);
        writer.Write(RecruitmentMinimumLevel);
        Demands.Write(writer);
    }
}
