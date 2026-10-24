using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Which marketplaces are disabled; the client keeps the status, raising ItemAuctionsDisabled or CommodityAuctionsDisabled as a bit
/// changes.
/// </summary>
/// <remarks>
/// While a bit is set the client offers no interaction with that kind of terminal, and closes it for a player using one.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0640, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MarketplaceStatusChanged
{
    /// <summary>
    /// The marketplaces disabled: <see cref="MarketplaceStatus.AuctionsDisabled"/> the auction house,
    /// <see cref="MarketplaceStatus.CommoditiesDisabled"/> the commodities exchange.
    /// </summary>
    [UInt(32)] public MarketplaceStatus Status { get; set; }
}
