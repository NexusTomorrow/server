using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires in response to MarketplaceLib.RequestOwnedCommodityOrders()
[Message(GameMessageOpcode.ServerOwnedCommodityOrders)]
public class ServerOwnedCommodityOrders : IWritable
{
    public List<CommodityOrder> Orders { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Orders.Count);
        Orders.ForEach(order => order.Write(writer));
    }
}
