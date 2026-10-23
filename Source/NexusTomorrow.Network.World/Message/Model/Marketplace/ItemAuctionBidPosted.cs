using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Reports a bid posted on an item auction; the client raises ItemAuctionBidPosted with the auction.
/// </summary>
[WireMessage(GameMessageOpcode.Server055F, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ItemAuctionBidPosted
{
    /// <summary>
    /// The auction.
    /// </summary>
    public ItemAuction Auction { get; set; } = new();
}
