using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class PublicEventTeamStats : IWritable
{
    [UInt(14)] public PublicEventTeam TeamId { get; set; }
    public PublicEventStats Stats { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TeamId, 14);
        Stats.Write(writer);
    }
}
