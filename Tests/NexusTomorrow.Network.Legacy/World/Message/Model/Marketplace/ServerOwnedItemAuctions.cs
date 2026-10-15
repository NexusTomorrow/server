using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires in response to MarketplaceLib.RequestOwnedItemAuctions()
[Message(GameMessageOpcode.ServerOwnedItemAuctions)]
public class ServerOwnedItemAuctions : IWritable
{
    public List<AuctionInfo> Auctions { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Auctions.Count);
        Auctions.ForEach(order => order.Write(writer));
    }
}
