using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace
{
    [Message(GameMessageOpcode.ClientAuctionSellOrderSubmit)]
    public class ClientAuctionSellOrderSubmit : IReadable
    {
        public ulong ItemGuid { get; private set; }
        public ulong MinimumBid { get; private set; }
        public ulong BuyoutPrice { get; private set; }

        public void Read(GamePacketReader reader)
        {
            ItemGuid = reader.ReadULong();
            MinimumBid = reader.ReadULong();
            BuyoutPrice = reader.ReadULong();
        }
    }
}
