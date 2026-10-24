using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// The player's own commodity orders; the client raises OwnedCommodityOrders with them.
/// </summary>
[WireMessage(GameMessageOpcode.Server064C, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class OwnedCommodityOrders
{
    /// <summary>
    /// The orders.
    /// </summary>
    [Array(CountBits = 32)] public List<CommodityOrder> Orders { get; set; } = [];
}
