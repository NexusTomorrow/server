using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Reports an item auction won; the client raises ItemAuctionWon with the auction.
/// </summary>
[WireMessage(GameMessageOpcode.Server0564, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ItemAuctionWon
{
    /// <summary>
    /// The auction.
    /// </summary>
    public ItemAuction Auction { get; set; } = new();
}
