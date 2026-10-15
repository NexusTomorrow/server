using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Sent from ConfirmButtonType == MarketplaceCommoditiesSubmit
[WireMessage(GameMessageOpcode.ClientCommoditySellOrderSubmit, Direction.ClientToServer)]
public sealed partial class ClientCommoditySellOrderSubmit
{
    public CommodityOrder Order { get; private set; } = new();
}
