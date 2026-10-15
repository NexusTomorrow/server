using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.ServerAuctionCancelResult, Direction.ServerToClient)]
public sealed partial class ServerAuctionCancelResult
{
    public AuctionInfo Auction { get; set; }
    [UInt(8)] public GenericError Result { get; set; }
}
