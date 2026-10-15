using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[Message(GameMessageOpcode.ServerAuctionCancelResult)]
public class ServerAuctionCancelResult : IWritable
{
    public AuctionInfo Auction { get; set; }
    public GenericError Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Auction.Write(writer);
        writer.Write(Result, 8u);
    }
}
