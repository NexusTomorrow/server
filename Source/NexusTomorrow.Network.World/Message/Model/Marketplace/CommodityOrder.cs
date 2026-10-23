using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// A commodity order: one order to buy or to sell a count of one item at a price per unit, which the client hands to add-ons as a
/// Game.CommodityOrder.
/// </summary>
[WireStruct]
public sealed partial class CommodityOrder
{
    /// <summary>
    /// The order, by id; <see cref="CommodityOrderCancel"/> sends it back to cancel the order.
    /// </summary>
    [UInt(64)] public ulong OrderId { get; set; }

    /// <summary>
    /// The item, an Item2 table id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; set; }

    /// <summary>
    /// How many units of the item the order is for.
    /// </summary>
    [UInt(32)] public uint Count { get; set; }

    /// <summary>
    /// The price of one unit, in credits.
    /// </summary>
    [UInt(64)] public ulong PricePerUnit { get; set; }

    [UInt(64)] public ulong Unknown4 { get; set; }

    /// <summary>
    /// Whether the order buys; otherwise it sells.
    /// </summary>
    [UInt(1)] public bool IsBuy { get; set; }

    /// <summary>
    /// The value the order's IsForceImmediate getter returns.
    /// </summary>
    [UInt(1)] public bool IsForceImmediate { get; set; }

    /// <summary>
    /// The order's list time, as its GetListTime getter returns it: a Windows FILETIME, 100-nanosecond intervals since 1601-01-01 UTC,
    /// which the client keeps as is.
    /// </summary>
    [UInt(64)] public ulong ListTime { get; set; }

    /// <summary>
    /// When the order expires: a Windows FILETIME, 100-nanosecond intervals since 1601-01-01 UTC, which the client keeps as is and hands
    /// to add-ons as a Time.
    /// </summary>
    [UInt(64)] public ulong ExpirationTime { get; set; }
}
