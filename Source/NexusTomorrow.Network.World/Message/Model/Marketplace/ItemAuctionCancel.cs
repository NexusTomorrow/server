using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Cancels one of the player's item auctions.
/// </summary>
/// <remarks>
/// The client refuses to send it for an auction the player does not own, and copies each field from the auction.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0094, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ItemAuctionCancel
{
    /// <summary>
    /// The auction's item, an Item2 table id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; private set; }

    /// <summary>
    /// The auction, by <see cref="ItemAuction.AuctionId"/>.
    /// </summary>
    [UInt(64)] public ulong AuctionId { get; private set; }
}
