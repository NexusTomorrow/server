using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[Message(GameMessageOpcode.ServerAuctionExpired)]
[WireMessage(GameMessageOpcode.ServerAuctionExpired, Direction.ServerToClient)]
public sealed partial class ServerAuctionExpired : IWritable
{
    public AuctionInfo Auction { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Auction.Write(writer);
    }
}
