using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[Message(GameMessageOpcode.ServerAuctionOutbid)]
[WireMessage(GameMessageOpcode.ServerAuctionOutbid, Direction.ServerToClient)]
public sealed partial class ServerAuctionOutbid : IWritable
{
    public AuctionInfo Auction { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Auction.Write(writer);
    }
}
