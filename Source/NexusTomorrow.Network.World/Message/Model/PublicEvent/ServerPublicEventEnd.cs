using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerPublicEventEnd, Direction.ServerToClient)]
public sealed partial class ServerPublicEventEnd
{
    [WireStruct]
    public sealed partial class PublicEventObjectiveStatus
    {
        [UInt(15)] public uint ObjectiveId { get; set; }
        [UInt(32)] public PublicEventStatus Status { get; set; }
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
}
