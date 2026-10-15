using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventEnd)]
[WireMessage(GameMessageOpcode.ServerPublicEventEnd, Direction.ServerToClient)]
public sealed partial class ServerPublicEventEnd : IWritable
{
    [WireStruct]
    public sealed partial class PublicEventObjectiveStatus : IWritable
    {
        [UInt(15)] public uint ObjectiveId { get; set; }
        [UInt(32)] public PublicEventStatus Status { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ObjectiveId, 15);
            writer.Write(Status, 32);
        }
    }

    [UInt(14)] public uint PublicEventId { get; set; }
    [UInt(32)] public PublicEventRemoveReason Reason { get; set; }
    [UInt(32)] public uint ElapsedTimeMs { get; set; }
    public PublicEventStats PersonalStats { get; set; }
    [Array(CountBits = 32)] public List<PublicEventTeamStats> TeamStats { get; set; } = [];
    [Array(CountBits = 32)] public List<PublicEventParticipantStats> ParticipantStats { get; set; } = [];
    [Array(CountBits = 32)] public List<PublicEventObjectiveStatus> ObjectiveStatus { get; set; } = [];
    [UInt(32)] public PublicEventRewardTier RewardTier { get; set; }
    [UInt(32)] public PublicEventRewardType RewardType { get; set; }
    [FixedArray(3), UInt(32)] public uint[] RewardThreshold { get; set; } = new uint[3];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PublicEventId, 14);
        writer.Write(Reason, 32);
        writer.Write(ElapsedTimeMs);
        PersonalStats.Write(writer);

        writer.Write(TeamStats.Count);
        foreach (PublicEventTeamStats stats in TeamStats)
            stats.Write(writer);

        writer.Write(ParticipantStats.Count);
        foreach (PublicEventParticipantStats stats in ParticipantStats)
            stats.Write(writer);

        writer.Write(ObjectiveStatus.Count);
        foreach (PublicEventObjectiveStatus status in ObjectiveStatus)
            status.Write(writer);

        writer.Write(RewardTier, 32u);
        writer.Write(RewardType, 32u);

        foreach (uint threshold in RewardThreshold)
            writer.Write(threshold);
    }
}
