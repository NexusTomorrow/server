using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountItemAdd)]
public class ServerAccountItemAdd : IWritable
{
    public AccountInventoryItem Item { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Item.Write(writer);
    }
}
