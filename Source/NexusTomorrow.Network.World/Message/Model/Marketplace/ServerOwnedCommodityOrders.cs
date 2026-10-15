using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires in response to MarketplaceLib.RequestOwnedCommodityOrders()
[WireMessage(GameMessageOpcode.ServerOwnedCommodityOrders, Direction.ServerToClient)]
public sealed partial class ServerOwnedCommodityOrders
{
    [Array(CountBits = 32)] public List<CommodityOrder> Orders { get; set; } = [];
}
