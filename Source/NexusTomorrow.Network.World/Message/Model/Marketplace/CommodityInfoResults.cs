using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// One item's commodity market: its order counts, its average prices and its orders. The client raises CommodityInfoResults with the
/// item, the counts and prices, and the orders.
/// </summary>
[WireMessage(GameMessageOpcode.Server024C, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CommodityInfoResults
{
    /// <summary>
    /// The item, an Item2 table id, as <see cref="CommodityInfoRequest.ItemId"/> asked for it.
    /// </summary>
    [UInt(18)] public uint ItemId { get; set; }

    /// <summary>
    /// How many buy orders the item has.
    /// </summary>
    [UInt(32)] public uint BuyOrderCount { get; set; }

    /// <summary>
    /// The average prices of the best 1, 10 and 50 buy orders, in credits.
    /// </summary>
    [FixedArray(3), UInt(64)] public ulong[] BuyOrderPrices { get; set; } = new ulong[3];

    /// <summary>
    /// How many sell orders the item has.
    /// </summary>
    [UInt(32)] public uint SellOrderCount { get; set; }

    /// <summary>
    /// The average prices of the best 1, 10 and 50 sell orders, in credits.
    /// </summary>
    [FixedArray(3), UInt(64)] public ulong[] SellOrderPrices { get; set; } = new ulong[3];

    /// <summary>
    /// The orders.
    /// </summary>
    [Array(CountBits = 32)] public List<CommodityOrder> Orders { get; set; } = [];
}
