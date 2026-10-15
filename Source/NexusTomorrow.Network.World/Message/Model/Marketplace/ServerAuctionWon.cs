using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.ServerAuctionWon, Direction.ServerToClient)]
public sealed partial class ServerAuctionWon
{
    public AuctionInfo Auction { get; set; }
}
