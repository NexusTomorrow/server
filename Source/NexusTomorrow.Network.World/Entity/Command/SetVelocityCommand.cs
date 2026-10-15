using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetVelocity)]
[WireStruct]
[UnionCase(EntityCommand.SetVelocity)]
public sealed partial class SetVelocityCommand : IEntityCommandModel
{
    [PackedFloat] public Vector3 Velocity { get; set; }
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        Velocity = reader.ReadPackedVector3();
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.WritePackedVector3(Velocity);
        writer.Write(Blend);
    }
}
