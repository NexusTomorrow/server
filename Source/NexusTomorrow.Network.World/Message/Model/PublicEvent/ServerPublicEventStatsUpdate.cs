using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventStatsUpdate)]
[WireMessage(GameMessageOpcode.ServerPublicEventStatsUpdate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventStatsUpdate : IWritable
{
    [UInt(14)] public uint PublicEventId { get; set; }
    [Array(CountBits = 32)] public List<PublicEventTeamStats> TeamStats { get; set; } = [];
    [Array(CountBits = 32)] public List<PublicEventParticipantStats> ParticipantStats { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PublicEventId, 14);

        writer.Write(TeamStats.Count);
        foreach (PublicEventTeamStats stats in TeamStats)
            stats.Write(writer);

        writer.Write(ParticipantStats.Count);
        foreach (PublicEventParticipantStats stats in ParticipantStats)
            stats.Write(writer);
    }
}
