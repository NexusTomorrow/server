using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(AuctionFilterType.RuneSlot)]
public sealed partial class RuneSlotAuctionFilter : IAuctionFilter
{
    [UInt(5)] public byte MichrochipType { get; private set; }
    [UInt(8)] public byte Minimum { get; private set; }
}
