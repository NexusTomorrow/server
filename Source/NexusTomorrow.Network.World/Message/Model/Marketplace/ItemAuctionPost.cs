using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Client06DC, Direction.ClientToServer)]
public sealed partial class ItemAuctionPost
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [UInt(64)] public ulong Unknown2 { get; private set; }
}
