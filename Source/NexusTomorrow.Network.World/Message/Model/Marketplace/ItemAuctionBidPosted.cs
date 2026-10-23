using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server055F, Direction.ServerToClient)]
public sealed partial class ItemAuctionBidPosted
{
    public ItemAuction Unknown0 { get; set; } = new();
}
