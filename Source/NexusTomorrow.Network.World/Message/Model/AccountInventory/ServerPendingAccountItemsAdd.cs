using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory
{
    // Clears all items from client's PendingAccountItem list before adding these 
    [Message(GameMessageOpcode.ServerPendingAccountItemsAdd)]
    public class ServerPendingAccountItemsAdd : IWritable
    {
        public List<PendingAccountItem> PendingItems { get; set; } = [];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(PendingItems.Count);
            PendingItems.ForEach(w => w.Write(writer));
        }
    }
}
