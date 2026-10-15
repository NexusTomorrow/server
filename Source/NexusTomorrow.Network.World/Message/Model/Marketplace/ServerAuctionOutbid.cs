using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.ServerAuctionOutbid, Direction.ServerToClient)]
public sealed partial class ServerAuctionOutbid
{
    public AuctionInfo Auction { get; set; }
}
