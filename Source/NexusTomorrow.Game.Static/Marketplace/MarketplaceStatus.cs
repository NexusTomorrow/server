namespace NexusTomorrow.Game.Static.Marketplace;

[Flags]
public enum MarketplaceStatus
{
    /// <summary>
    /// The auction house is off: the client offers no interaction with an auction house terminal, and moves a player at one away from
    /// it.
    /// </summary>
    AuctionsDisabled = 0x01,

    /// <summary>
    /// The commodities exchange is off: the client offers no interaction with a commodities exchange terminal, and moves a player at
    /// one away from it.
    /// </summary>
    CommoditiesDisabled = 0x02,
}
