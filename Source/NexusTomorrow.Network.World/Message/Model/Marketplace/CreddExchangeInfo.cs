using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// The CREDD exchange's order counts and average prices, and CREDD orders; the client raises CREDDExchangeInfoResults with them.
/// </summary>
[WireMessage(GameMessageOpcode.Server026A, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CreddExchangeInfo
{
    /// <summary>
    /// A CREDD exchange order: one order to buy or to sell CREDD at a price, which the client hands to add-ons as a
    /// Game.CREDDExchangeOrder.
    /// </summary>
    [WireStruct]
    public sealed partial class CreddExchangeOrder
    {
        /// <summary>
        /// The order, by id; <see cref="CreddExchangeOrderCancel"/> sends it back to cancel the order.
        /// </summary>
        [UInt(64)] public ulong OrderId { get; set; }

        /// <summary>
        /// The order's price.
        /// </summary>
        [UInt(64)] public ulong Price { get; set; }

        /// <summary>
        /// Whether the order buys CREDD; otherwise it sells.
        /// </summary>
        [UInt(1)] public bool IsBuy { get; set; }

        /// <summary>
        /// The order's list time, as its GetListTime getter returns it; the client converts it to seconds since 1970 for add-ons.
        /// </summary>
        [UInt(64)] public ulong ListTime { get; set; }

        /// <summary>
        /// When the order expires; the client converts it to seconds since 1970 for add-ons.
        /// </summary>
        [UInt(64)] public ulong ExpirationTime { get; set; }
    }

    /// <summary>
    /// How many buy orders the exchange has.
    /// </summary>
    [UInt(32)] public uint BuyOrderCount { get; set; }

    /// <summary>
    /// The average prices of the best 1, 10 and 50 buy orders, in credits.
    /// </summary>
    [FixedArray(3), UInt(64)] public ulong[] BuyOrderPrices { get; set; } = new ulong[3];

    /// <summary>
    /// How many sell orders the exchange has.
    /// </summary>
    [UInt(32)] public uint SellOrderCount { get; set; }

    /// <summary>
    /// The average prices of the best 1, 10 and 50 sell orders, in credits.
    /// </summary>
    [FixedArray(3), UInt(64)] public ulong[] SellOrderPrices { get; set; } = new ulong[3];

    /// <summary>
    /// The orders.
    /// </summary>
    [Array(CountBits = 32)] public List<CreddExchangeOrder> Orders { get; set; } = [];
}
