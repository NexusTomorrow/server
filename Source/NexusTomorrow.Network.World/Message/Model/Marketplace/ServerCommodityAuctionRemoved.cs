using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires whenever a buy or sell order is removed from the Commodities Exchange.
[Message(GameMessageOpcode.ServerCommodityAuctionRemoved)]
public class ServerCommodityAuctionRemoved : IWritable
{
    public CommodityOrder OrderRemoved { get; set; } = new();
    public AuctionEventType Type { get; set; }

    public void Write(GamePacketWriter writer)
    {
        OrderRemoved.Write(writer);
        writer.Write(Type, 32u);
    }
}
