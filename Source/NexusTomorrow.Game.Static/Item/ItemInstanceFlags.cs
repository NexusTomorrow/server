namespace NexusTomorrow.Game.Static.Item;

/// <summary>
/// The flags an item carries, which the client tests bit by bit.
/// </summary>
[Flags]
public enum ItemInstanceFlags
{
    None = 0x00,

    /// <summary>
    /// The item is soulbound, as Item:IsSoulbound reports.
    /// </summary>
    Soulbound = 0x01,

    /// <summary>
    /// The item's vendor return window does not apply: the client leaves it out of the item's return time, restocking fee and sell price.
    /// </summary>
    NotReturnable = 0x08,

    Unknown10 = 0x10,

    /// <summary>
    /// The item cannot be sold: the client refuses the sale and gives the item no sell price.
    /// </summary>
    NotSellable = 0x20,

    /// <summary>
    /// The item may be traded within the account, as Item:IsAccountTradeable reports.
    /// </summary>
    AccountTradeable = 0x40,

    /// <summary>
    /// The item cannot be traded: the client refuses it in a trade, and its tooltip says so.
    /// </summary>
    NoTrade = 0x80,
}
