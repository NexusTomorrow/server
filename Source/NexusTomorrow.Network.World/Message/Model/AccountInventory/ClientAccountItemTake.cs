using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory
{
    [Message(GameMessageOpcode.ClientAccountItemTake)]
    public class ClientAccountItemTake : IReadable
    {
        public ulong UserInventoryId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            UserInventoryId = reader.ReadULong();
        }
    }
}
