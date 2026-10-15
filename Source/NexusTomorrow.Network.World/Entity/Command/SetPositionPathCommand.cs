using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Spline;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetPositionPath)]
[WireStruct]
[UnionCase(EntityCommand.SetPositionPath)]
public sealed partial class SetPositionPathCommand : IEntityCommandModel
{
    [Array(CountBits = 10), Float32] public List<Vector3> Positions { get; set; } = [];
    [PackedFloat] public float Speed { get; set; }
    [UInt(2)] public SplineType Type { get; set; }
    [UInt(4)] public SplineMode Mode { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadUShort(10u);
        for (int i = 0; i < count; i++)
            Positions.Add(reader.ReadVector3());

        Speed = reader.ReadPackedFloat();
        Type = reader.ReadEnum<SplineType>(2u);
        Mode = reader.ReadEnum<SplineMode>(4u);
        Offset = reader.ReadUInt();
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Positions.Count, 10u);
        Positions.ForEach(writer.WriteVector3);

        writer.WritePackedFloat(Speed);
        writer.Write(Type, 2u);
        writer.Write(Mode, 4u);
        writer.Write(Offset);
        writer.Write(Blend);
    }
}
