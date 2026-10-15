using NexusTomorrow.Game.Static.Hazard;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireStruct]
public sealed partial class Hazard : IWritable
{
    [UInt(14)] public ushort HazardId { get; set; }
    [UInt(3)] public HazardType Type { get; set; }
    [Float32] public float MeterValue { get; set; }
    [Float32] public float MaxValue { get; set; }
    [UInt(32)] public uint CurrentThreshold { get; set; }
    [UInt(18)] public uint ProcSpell4Id { get; set; }
    [UInt(32)] public uint HazardUnitId { get; set; }
    [UInt(32)] public uint PulseTimeLeft { get; set; }
    [UInt(1)] public bool UnitBased { get; set; }
    [UInt(1)] public bool StartsFull { get; set; }
    [UInt(1)] public bool Enabled { get; set; }
    [UInt(1)] public bool Suspended { get; set; }
    [UInt(1)] public bool DoNotRefill { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(HazardId, 14u);
        writer.Write(Type, 3u);
        writer.Write(MeterValue);
        writer.Write(MaxValue);
        writer.Write(CurrentThreshold, 32u);
        writer.Write(ProcSpell4Id, 18u);
        writer.Write(HazardUnitId);
        writer.Write(PulseTimeLeft);
        writer.Write(UnitBased);
        writer.Write(StartsFull);
        writer.Write(Enabled);
        writer.Write(Suspended);
        writer.Write(DoNotRefill);
    }
}
