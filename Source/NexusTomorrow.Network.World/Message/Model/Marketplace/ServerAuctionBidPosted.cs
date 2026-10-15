using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.ServerAuctionBidPosted, Direction.ServerToClient)]
public sealed partial class ServerAuctionBidPosted
{
    public AuctionInfo Auction { get; set; }
}
