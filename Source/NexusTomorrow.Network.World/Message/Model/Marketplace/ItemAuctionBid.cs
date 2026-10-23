using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Bids on an item auction. A buyout is a bid of the auction's buyout price.
/// </summary>
/// <remarks>
/// The client refuses to send it for the player's own auction.
/// </remarks>
[WireMessage(GameMessageOpcode.Client055E, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ItemAuctionBid
{
    /// <summary>
    /// The auction's item, an Item2 table id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; private set; }

    /// <summary>
    /// The auction, by <see cref="ItemAuction.AuctionId"/>.
    /// </summary>
    [UInt(64)] public ulong AuctionId { get; private set; }

    /// <summary>
    /// The amount bid: the add-on's bid price, or the auction's <see cref="ItemAuction.BuyoutPrice"/> for a buyout.
    /// </summary>
    [UInt(64)] public ulong Amount { get; private set; }
}
