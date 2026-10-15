using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerBuybackItems)]
public class ServerBuybackItems : IWritable
{
    public List<BuybackItem> BuybackItems { get; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(BuybackItems.Count);
        BuybackItems.ForEach(b => b.Write(writer));
    }
}
