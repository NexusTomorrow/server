using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Client055E, Direction.ClientToServer)]
public sealed partial class ItemAuctionBid
{
    [UInt(18)] public uint Unknown0 { get; private set; }
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [UInt(64)] public ulong Unknown2 { get; private set; }
}
