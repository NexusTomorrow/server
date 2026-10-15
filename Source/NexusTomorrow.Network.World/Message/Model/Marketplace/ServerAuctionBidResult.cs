using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.ServerAuctionBidResult, Direction.ServerToClient)]
public sealed partial class ServerAuctionBidResult
{
    [UInt(8)] public GenericError Result { get; set; }
    public AuctionInfo Auction { get; set; }
}
