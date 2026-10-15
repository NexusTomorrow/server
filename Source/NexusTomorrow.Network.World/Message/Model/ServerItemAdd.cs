using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerItemAdd)]
public class ServerItemAdd : IWritable
{
    public InventoryItem InventoryItem { get; set; }

    public void Write(GamePacketWriter writer)
    {
        InventoryItem.Write(writer);
    }
}
