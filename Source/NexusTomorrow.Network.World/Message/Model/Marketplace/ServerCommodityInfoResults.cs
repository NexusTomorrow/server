using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires for each item returned by MarketplaceLib.RequestCommodityInfo().
// It contains information on all of the buy and sell orders out for the item.
[WireMessage(GameMessageOpcode.ServerCommodityInfoResults, Direction.ServerToClient)]
public sealed partial class ServerCommodityInfoResults
{
    [UInt(18)] public uint Item2Id { get; set; }
    [UInt(32)] public uint BuyOrderCount { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] BuyOrderPrices { get; set; } = new ulong[3]; // Price for each order quantity bucket (1, 10, 50 units)
    [UInt(32)] public uint SellOrderCount { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] SellOrderPrices { get; set; } = new ulong[3]; // Price for each order quantity bucket (1, 10, 50 units)
    [Array(CountBits = 32)] public List<CommodityOrder> Orders { get; set; } = [];
}
