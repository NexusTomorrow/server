using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server0562, Direction.ServerToClient)]
public sealed partial class ItemAuctionOutbid
{
    public ItemAuction Unknown0 { get; set; } = new();
}
