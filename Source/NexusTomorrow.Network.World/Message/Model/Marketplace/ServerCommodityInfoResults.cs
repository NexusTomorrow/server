using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires for each item returned by MarketplaceLib.RequestCommodityInfo().
// It contains information on all of the buy and sell orders out for the item.
[Message(GameMessageOpcode.ServerCommodityInfoResults)]
[WireMessage(GameMessageOpcode.ServerCommodityInfoResults, Direction.ServerToClient)]
public sealed partial class ServerCommodityInfoResults : IWritable
{
    [UInt(18)] public uint Item2Id { get; set; }
    [UInt(32)] public uint BuyOrderCount { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] BuyOrderPrices { get; set; } = new ulong[3]; // Price for each order quantity bucket (1, 10, 50 units)
    [UInt(32)] public uint SellOrderCount { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] SellOrderPrices { get; set; } = new ulong[3]; // Price for each order quantity bucket (1, 10, 50 units)
    [Array(CountBits = 32)] public List<CommodityOrder> Orders { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Item2Id, 18u);
        writer.Write(BuyOrderCount);

        for (int i = 0; i < BuyOrderPrices.Length; i++)
        {
            writer.Write(BuyOrderPrices[i]);
        }
        writer.Write(SellOrderCount);

        for (int i = 0; i < SellOrderPrices.Length; i++)
        {
            writer.Write(SellOrderPrices[i]);
        }

        writer.Write(Orders.Count);
        Orders.ForEach(order => order.Write(writer));
    }
}
