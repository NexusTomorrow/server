using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace
{
    // MarketplaceLib.RequestOwnedCommodityOrders()
    [Message(GameMessageOpcode.ClientRequestOwnedCommodityOrders)]
    public class ClientRequestOwnedCommodityOrders : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            // zero byte message
        }
    }
}
