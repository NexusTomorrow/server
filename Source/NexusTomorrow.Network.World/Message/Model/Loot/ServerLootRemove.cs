using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot
{
    [Message(GameMessageOpcode.ServerLootRemove)]
    public class ServerLootRemove : IWritable
    {
        public uint OwnerUnitId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(OwnerUnitId);
        }
    }
}
