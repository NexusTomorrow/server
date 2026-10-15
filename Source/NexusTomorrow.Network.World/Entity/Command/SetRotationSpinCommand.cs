using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetRotationSpin)]
[WireStruct]
public sealed partial class SetRotationSpinCommand : IEntityCommandModel
{
    [Float32] public Vector3 Rotation { get; set; }
    [UInt(32)] public uint FlightTime { get; set; }
    [PackedFloat] public float Spin { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        Rotation = reader.ReadVector3();
        FlightTime = reader.ReadUInt();
        Spin = reader.ReadPackedFloat();
        Offset = reader.ReadUInt();
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteVector3(Rotation);
        writer.Write(FlightTime);
        writer.WritePackedFloat(Spin);
        writer.Write(Offset);
        writer.Write(Blend);
    }
}
