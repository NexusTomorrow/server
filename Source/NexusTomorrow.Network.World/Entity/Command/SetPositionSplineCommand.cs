using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Spline;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetPositionSpline)]
[WireStruct]
[UnionCase(EntityCommand.SetPositionSpline)]
public sealed partial class SetPositionSplineCommand : IEntityCommandModel
{
    [UInt(32)] public uint SplineId { get; set; }
    [Float32] public float Speed { get; set; }
    [Float32] public float Position { get; set; }
    [PackedFloat] public Vector3 FormationData { get; set; }
    [UInt(4)] public SplineMode Mode { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }
    [UInt(1)] public bool IsContinuing { get; set; }
    [UInt(1)] public bool AdjustSpeedToLength { get; set; }

    public void Read(GamePacketReader reader)
    {
        SplineId = reader.ReadUInt();
        Speed = reader.ReadSingle();
        Position = reader.ReadSingle();
        FormationData = reader.ReadPackedVector3();
        Mode = reader.ReadEnum<SplineMode>(4u);
        Offset = reader.ReadUInt();
        Blend = reader.ReadBit();
        IsContinuing = reader.ReadBit();
        AdjustSpeedToLength = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SplineId);
        writer.Write(Speed);
        writer.Write(Position);
        writer.WritePackedVector3(FormationData);
        writer.Write(Mode, 4u);
        writer.Write(Offset);
        writer.Write(Blend);
        writer.Write(IsContinuing);
        writer.Write(AdjustSpeedToLength);
    }
}
