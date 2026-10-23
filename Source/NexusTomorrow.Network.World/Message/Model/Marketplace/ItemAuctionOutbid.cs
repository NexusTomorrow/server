using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Reports an item auction outbid; the client raises ItemAuctionOutbid with the auction.
/// </summary>
[WireMessage(GameMessageOpcode.Server0562, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ItemAuctionOutbid
{
    /// <summary>
    /// The auction.
    /// </summary>
    public ItemAuction Auction { get; set; } = new();
}
