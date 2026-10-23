using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server064C, Direction.ServerToClient)]
public sealed partial class OwnedCommodityOrders
{
    [Array(CountBits = 32)] public List<CommodityOrder> Unknown0 { get; set; } = [];
}
