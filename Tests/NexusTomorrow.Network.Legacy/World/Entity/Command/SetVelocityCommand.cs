using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetVelocity)]
public class SetVelocityCommand : IEntityCommandModel
{
    public Vector3 Velocity { get; set; }
    public bool Blend { get; set; }

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
