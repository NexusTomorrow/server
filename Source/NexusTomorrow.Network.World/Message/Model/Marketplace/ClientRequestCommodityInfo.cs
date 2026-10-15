using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// MarketplaceLib::RequestCommodityInfo()
[WireMessage(GameMessageOpcode.ClientRequestCommodityInfo, Direction.ClientToServer)]
public sealed partial class ClientRequestCommodityInfo
{
    [UInt(18)] public uint Item2Id { get; private set; }
}
