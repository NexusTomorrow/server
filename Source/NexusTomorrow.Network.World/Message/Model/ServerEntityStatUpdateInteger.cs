using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Sent to set an integer stat. The layout matches <see cref="ServerEntityStatUpdateFloat"/> except that the value is an integer,
/// so the declaration carries it in <see cref="IntegerStat"/>; the legacy <see cref="Stat"/> stays for the legacy writer.
/// </summary>
[Message(GameMessageOpcode.ServerEntityStatUpdateInteger)]
[WireMessage(GameMessageOpcode.ServerEntityStatUpdateInteger, Direction.ServerToClient)]
public sealed partial class ServerEntityStatUpdateInteger : IWritable
{
    [WireStruct]
    public sealed partial class IntegerStatValue
    {
        [UInt(5)] public Stat Stat { get; set; }
        [UInt(32)] public uint Value { get; set; }
    }

    [UInt(32)] public uint UnitId { get; set; }
    public IntegerStatValue IntegerStat { get; set; } = new();
    [NotOnWire] public StatValueUpdate Stat { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        Stat.Write(writer);
    }
}
