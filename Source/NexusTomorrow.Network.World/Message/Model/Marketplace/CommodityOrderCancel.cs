using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Client0093, Direction.ClientToServer)]
public sealed partial class CommodityOrderCancel
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [UInt(18)] public uint Unknown1 { get; private set; }
    [UInt(1)] public bool Unknown2 { get; private set; }
}
