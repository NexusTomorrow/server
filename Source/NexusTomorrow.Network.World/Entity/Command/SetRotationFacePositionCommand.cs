using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetRotationFacePosition)]
[WireStruct]
[UnionCase(EntityCommand.SetRotationFacePosition)]
public sealed partial class SetRotationFacePositionCommand : IEntityCommandModel
{
    [Float32] public Vector3 Position { get; set; }
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        Position = reader.ReadVector3();
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteVector3(Position);
        writer.Write(Blend);
    }
}
