using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

public class PublicEventTeamStats : IWritable
{
    public PublicEventTeam TeamId { get; set; }
    public PublicEventStats Stats { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TeamId, 14);
        Stats.Write(writer);
    }
}
