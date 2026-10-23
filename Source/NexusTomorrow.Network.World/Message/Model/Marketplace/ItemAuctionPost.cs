using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Posts an item from the player's inventory for auction.
/// </summary>
/// <remarks>
/// The client sends it only for an item that can be auctioned, with a minimum bid above zero and a buyout of 0 or not below the bid.
/// </remarks>
[WireMessage(GameMessageOpcode.Client06DC, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ItemAuctionPost
{
    /// <summary>
    /// The item to auction, by id, from the player's inventory.
    /// </summary>
    [UInt(64)] public ulong ItemGuid { get; private set; }

    /// <summary>
    /// The minimum bid, in credits, above zero.
    /// </summary>
    [UInt(64)] public ulong MinimumBid { get; private set; }

    /// <summary>
    /// The buyout price: 0, or not below <see cref="MinimumBid"/>.
    /// </summary>
    [UInt(64)] public ulong BuyoutPrice { get; private set; }
}
