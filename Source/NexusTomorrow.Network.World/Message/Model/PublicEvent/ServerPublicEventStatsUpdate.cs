using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerPublicEventStatsUpdate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventStatsUpdate
{
    [UInt(14)] public uint PublicEventId { get; set; }
    [Array(CountBits = 32)] public List<PublicEventTeamStats> TeamStats { get; set; } = [];
    [Array(CountBits = 32)] public List<PublicEventParticipantStats> ParticipantStats { get; set; } = [];
}
