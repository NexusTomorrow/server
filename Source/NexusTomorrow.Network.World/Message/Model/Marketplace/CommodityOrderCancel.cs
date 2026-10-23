using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Cancels one of the player's commodity orders.
/// </summary>
/// <remarks>
/// The client refuses to send it for an order that is not posted, and copies each field from the order.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0093, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CommodityOrderCancel
{
    /// <summary>
    /// The order, by <see cref="CommodityOrder.OrderId"/>.
    /// </summary>
    [UInt(64)] public ulong OrderId { get; private set; }

    /// <summary>
    /// The order's item, an Item2 table id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; private set; }

    /// <summary>
    /// Whether the order buys, as <see cref="CommodityOrder.IsBuy"/>.
    /// </summary>
    [UInt(1)] public bool IsBuy { get; private set; }
}
