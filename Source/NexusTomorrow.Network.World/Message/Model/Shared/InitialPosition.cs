using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Shared
{
    public class InitialPosition : IWritable
    {
        public uint     UnitId { get; set; }
        public byte     TargetFlags { get; set; }
        public Position Position { get; set; } = new Position();
        public float    Yaw { get; set; }
        public float    Pitch { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(UnitId);
            writer.Write(TargetFlags);
            Position.Write(writer);
            writer.Write(Yaw);
            writer.Write(Pitch);
        }
    }
}
