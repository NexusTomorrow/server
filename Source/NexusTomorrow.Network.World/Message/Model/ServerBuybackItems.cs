using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerBuybackItems)]
[WireMessage(GameMessageOpcode.ServerBuybackItems, Direction.ServerToClient)]
public sealed partial class ServerBuybackItems : IWritable
{
    [Array(CountBits = 32)] public List<BuybackItem> BuybackItems { get; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(BuybackItems.Count);
        BuybackItems.ForEach(b => b.Write(writer));
    }
}
