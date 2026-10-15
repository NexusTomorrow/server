using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// MarketplaceLib.RequestOwnedCommodityOrders()
[Message(GameMessageOpcode.ClientRequestOwnedCommodityOrders)]
[WireMessage(GameMessageOpcode.ClientRequestOwnedCommodityOrders, Direction.ClientToServer)]
public sealed partial class ClientRequestOwnedCommodityOrders : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
