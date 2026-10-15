using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.ServerAuctionExpired, Direction.ServerToClient)]
public sealed partial class ServerAuctionExpired
{
    public AuctionInfo Auction { get; set; }
}
