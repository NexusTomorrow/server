using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// An item auction: its owner, its prices and bids, the time it has left and the item, which the client hands to add-ons as a
/// Game.ItemAuction.
/// </summary>
[WireStruct]
public sealed partial class ItemAuction
{
    /// <summary>
    /// The auction, by id; <see cref="ItemAuctionBid"/> and <see cref="ItemAuctionCancel"/> send it back.
    /// </summary>
    [UInt(64)] public ulong AuctionId { get; set; }

    /// <summary>
    /// The character that owns the auction; the client counts the auction the player's own when this is the player's character id.
    /// </summary>
    [UInt(64)] public ulong OwnerCharacterId { get; set; }

    /// <summary>
    /// The auction's minimum bid.
    /// </summary>
    [UInt(64)] public ulong MinimumBid { get; set; }

    /// <summary>
    /// The auction's buyout price.
    /// </summary>
    [UInt(64)] public ulong BuyoutPrice { get; set; }

    /// <summary>
    /// The auction's current bid.
    /// </summary>
    [UInt(64)] public ulong CurrentBid { get; set; }

    /// <summary>
    /// The character with the top bid.
    /// </summary>
    [UInt(64)] public ulong TopBidderCharacterId { get; set; }

    /// <summary>
    /// How many seconds the auction has left.
    /// </summary>
    [UInt(64)] public ulong SecondsRemaining { get; set; }

    /// <summary>
    /// The item, an Item2 table id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; set; }

    /// <summary>
    /// How many of the item the auction is for.
    /// </summary>
    [UInt(32)] public uint Count { get; set; }

    [UInt(18)] public uint Unknown9 { get; set; }
    [Array(CountBits = 3), UInt(32)] public List<uint> Unknown10 { get; set; } = [];
    [UInt(64)] public ulong Unknown11 { get; set; }
    [UInt(32)] public uint Unknown12 { get; set; }
    [UInt(64)] public ulong Unknown13 { get; set; }
    [UInt(32)] public uint Unknown14 { get; set; }
}
