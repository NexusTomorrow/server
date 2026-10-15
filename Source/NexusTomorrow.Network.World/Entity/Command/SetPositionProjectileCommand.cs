using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetPositionProjectile)]
[WireStruct]
[UnionCase(EntityCommand.SetPositionProjectile)]
public sealed partial class SetPositionProjectileCommand : IEntityCommandModel
{
    [Float32] public Vector3 Position { get; set; }
    [Float32] public Vector3 Rotation { get; set; }
    [UInt(32)] public uint FlightTime { get; set; }
    [Float32] public float Gravity { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        Position = reader.ReadVector3();
        Rotation = reader.ReadVector3();
        FlightTime = reader.ReadUInt();
        Gravity = reader.ReadUInt();
        Offset = reader.ReadUInt();
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteVector3(Position);
        writer.WriteVector3(Rotation);
        writer.Write(FlightTime);
        writer.Write(Gravity);
        writer.Write(Offset);
        writer.Write(Blend);
    }
}
