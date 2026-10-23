using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Client03E6, Direction.ClientToServer)]
public sealed partial class CommodityInfoRequest
{
    [UInt(18)] public uint Unknown0 { get; private set; }
}
