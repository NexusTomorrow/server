using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetRotationMultiSpline)]
[WireStruct]
[UnionCase(EntityCommand.SetRotationMultiSpline)]
public sealed partial class SetRotationMultiSplineCommand : IEntityCommandModel
{
    [Array(CountBits = 10), UInt(32)] public List<uint> SplineIds { get; set; } = [];

    [PackedFloat] public float Speed { get; set; }
    [Float32] public float Position { get; set; }
    [Float32] public float TakeoffLocationHeight { get; set; }
    [Float32] public float LandingLocationHeight { get; set; }

    [UInt(4)] public byte Mode { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(32)] public uint MultiSplineFlags { get; set; }
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadUShort(10u);
        for (int i = 0; i < count; i++)
            SplineIds.Add(reader.ReadUInt());

        Speed = reader.ReadPackedFloat();
        Position = reader.ReadUInt();
        TakeoffLocationHeight = reader.ReadUInt();
        LandingLocationHeight = reader.ReadUInt();
        Mode = reader.ReadByte(4u);
        Offset = reader.ReadUInt();
        MultiSplineFlags = reader.ReadUInt();
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SplineIds.Count, 10u);
        foreach (uint splineId in SplineIds)
            writer.Write(splineId);

        writer.WritePackedFloat(Speed);
        writer.Write(Position);
        writer.Write(TakeoffLocationHeight);
        writer.Write(LandingLocationHeight);
        writer.Write(Mode, 4);
        writer.Write(Offset);
        writer.Write(MultiSplineFlags);
        writer.Write(Blend);
    }
}
