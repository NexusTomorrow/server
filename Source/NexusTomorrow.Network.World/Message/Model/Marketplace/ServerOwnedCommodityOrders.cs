using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires in response to MarketplaceLib.RequestOwnedCommodityOrders()
[Message(GameMessageOpcode.ServerOwnedCommodityOrders)]
[WireMessage(GameMessageOpcode.ServerOwnedCommodityOrders, Direction.ServerToClient)]
public sealed partial class ServerOwnedCommodityOrders : IWritable
{
    [Array(CountBits = 32)] public List<CommodityOrder> Orders { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Orders.Count);
        Orders.ForEach(order => order.Write(writer));
    }
}
