using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerUnitInUse)]
    public class ServerUnitInUse : IWritable
    {
        public uint UnitId { get; set; }
        public bool InUse { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(UnitId);
            writer.Write(InUse);
        }
    }
}
