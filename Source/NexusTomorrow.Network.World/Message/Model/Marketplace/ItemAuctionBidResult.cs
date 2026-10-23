using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// The result of a bid on an item auction; the client raises ItemAuctionBidResult with the result and the auction.
/// </summary>
[WireMessage(GameMessageOpcode.Server0560, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ItemAuctionBidResult
{
    /// <summary>
    /// The result, a GameLib.CodeEnumGenericError code.
    /// </summary>
    [UInt(8)] public byte Result { get; set; }

    /// <summary>
    /// The auction.
    /// </summary>
    public ItemAuction Auction { get; set; } = new();
}
