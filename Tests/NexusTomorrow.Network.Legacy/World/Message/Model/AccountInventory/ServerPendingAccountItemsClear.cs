using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

// Clears all items from client's PendingAccountItem list
[Message(GameMessageOpcode.ServerPendingAccountItemsClear)]
public class ServerPendingAccountItemsClear : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // zero byte message
    }
}
