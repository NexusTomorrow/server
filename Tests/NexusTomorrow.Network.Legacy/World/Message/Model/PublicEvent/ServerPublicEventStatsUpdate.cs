using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventStatsUpdate)]
public class ServerPublicEventStatsUpdate : IWritable
{
    public uint PublicEventId { get; set; }
    public List<PublicEventTeamStats> TeamStats { get; set; } = [];
    public List<PublicEventParticipantStats> ParticipantStats { get; set; } = [];

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
