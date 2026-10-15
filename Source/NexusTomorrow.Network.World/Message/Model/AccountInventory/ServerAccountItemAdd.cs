using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountItemAdd)]
[WireMessage(GameMessageOpcode.ServerAccountItemAdd, Direction.ServerToClient)]
public sealed partial class ServerAccountItemAdd : IWritable
{
    public AccountInventoryItem Item { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Item.Write(writer);
    }
}
