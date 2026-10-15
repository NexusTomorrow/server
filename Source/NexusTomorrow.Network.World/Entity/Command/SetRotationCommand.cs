using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;

namespace NexusTomorrow.Network.World.Entity.Command
{
    [EntityCommand(EntityCommand.SetRotation)]
    public class SetRotationCommand : IEntityCommandModel
    {
        public Vector3 Rotation { get; set; }
        public bool Blend { get; set; }

        public void Read(GamePacketReader reader)
        {
            Rotation = reader.ReadVector3();
            Blend    = reader.ReadBit();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteVector3(Rotation);
            writer.Write(Blend);
        }
    }
}
