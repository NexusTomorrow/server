using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// One page of auction search results; the client raises ItemAuctionSearchResults with the page, the total and the auctions.
/// </summary>
[WireMessage(GameMessageOpcode.Server0563, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ItemAuctionSearchResults
{
    /// <summary>
    /// How many auctions the search found, over every page.
    /// </summary>
    [UInt(32)] public uint TotalResults { get; set; }

    /// <summary>
    /// The page these results are.
    /// </summary>
    [UInt(32)] public uint Page { get; set; }

    /// <summary>
    /// The page's auctions.
    /// </summary>
    [Array(CountBits = 32)] public List<ItemAuction> Auctions { get; set; } = [];
}
