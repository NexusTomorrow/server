using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountItems)]
[WireMessage(GameMessageOpcode.ServerAccountItems, Direction.ServerToClient)]
public sealed partial class ServerAccountItems : IWritable
{
    [Array(CountBits = 32)] public List<AccountInventoryItem> AccountItems { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountItems.Count);
        AccountItems.ForEach(w => w.Write(writer));
    }
}
