using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Reports an item auction expired; the client raises ItemAuctionExpired with the auction.
/// </summary>
[WireMessage(GameMessageOpcode.Server0561, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ItemAuctionExpired
{
    /// <summary>
    /// The auction.
    /// </summary>
    public ItemAuction Auction { get; set; } = new();
}
