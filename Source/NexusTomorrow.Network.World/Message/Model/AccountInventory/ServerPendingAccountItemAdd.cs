using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerPendingAccountItemAdd)]
public class ServerPendingAccountItemAdd : IWritable
{
    public PendingAccountItem PendingItem { get; set; }

    public void Write(GamePacketWriter writer)
    {
        PendingItem.Write(writer);
    }
}
