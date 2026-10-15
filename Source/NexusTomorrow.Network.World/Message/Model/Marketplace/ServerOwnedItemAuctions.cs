using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires in response to MarketplaceLib.RequestOwnedItemAuctions()
[WireMessage(GameMessageOpcode.ServerOwnedItemAuctions, Direction.ServerToClient)]
public sealed partial class ServerOwnedItemAuctions
{
    [Array(CountBits = 32)] public List<AuctionInfo> Auctions { get; set; } = [];
}
