using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class PublicEventParticipantStats : IWritable
{
    [UInt(14)] public PublicEventTeam TeamId { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    public Identity Player { get; set; }
    [UInt(8)] public Class Class { get; set; }
    [Pad(24)] public Pad ClassHigh { get; }
    [UInt(8)] public Game.Static.PlayerPath.Path Path { get; set; }
    [Pad(24)] public Pad PathHigh { get; }
    public PublicEventStats Stats { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TeamId, 14);
        writer.Write(UnitId);
        Player.Write(writer);
        writer.Write(Class, 32);
        writer.Write(Path, 32);
        Stats.Write(writer);
    }
}
