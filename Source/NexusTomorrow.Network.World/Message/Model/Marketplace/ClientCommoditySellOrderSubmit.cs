using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace
{
    // Sent from ConfirmButtonType == MarketplaceCommoditiesSubmit
    [Message(GameMessageOpcode.ClientCommoditySellOrderSubmit)]
    public class ClientCommoditySellOrderSubmit : IReadable
    {
        public CommodityOrder Order { get; private set; } = new();

        public void Read(GamePacketReader reader)
        {
            Order.Read(reader);
        }
    }
}
