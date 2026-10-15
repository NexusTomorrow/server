using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

public class InventoryItem : IWritable
{
    public Item Item { get; set; }
    public ItemUpdateReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Item.Write(writer);
        writer.Write(Reason, 6u);
    }
}
