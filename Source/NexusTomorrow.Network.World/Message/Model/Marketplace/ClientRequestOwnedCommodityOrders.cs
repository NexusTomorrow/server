using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// MarketplaceLib.RequestOwnedCommodityOrders()
[WireMessage(GameMessageOpcode.ClientRequestOwnedCommodityOrders, Direction.ClientToServer)]
public sealed partial class ClientRequestOwnedCommodityOrders
{
}
