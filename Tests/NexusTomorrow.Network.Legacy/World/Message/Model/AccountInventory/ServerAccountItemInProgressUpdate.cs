using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountItemInProgressUpdate)]
public class ServerAccountItemInProgressUpdate : IWritable
{
    public uint AccountId { get; set; } // is the AccountId sent ServerRealmInfo (0x3DB), filled by server but not used by client
    public AccountInventoryItem Item { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountId);
        Item.Write(writer);
    }
}
