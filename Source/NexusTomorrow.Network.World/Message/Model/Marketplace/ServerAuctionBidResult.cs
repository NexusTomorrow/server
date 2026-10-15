using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[Message(GameMessageOpcode.ServerAuctionBidResult)]
[WireMessage(GameMessageOpcode.ServerAuctionBidResult, Direction.ServerToClient)]
public sealed partial class ServerAuctionBidResult : IWritable
{
    [UInt(8)] public GenericError Result { get; set; }
    public AuctionInfo Auction { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 8u);
        Auction.Write(writer);
    }
}
