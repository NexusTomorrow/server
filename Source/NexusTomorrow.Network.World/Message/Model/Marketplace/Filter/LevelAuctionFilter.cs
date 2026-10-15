using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(AuctionFilterType.Level)]
public sealed partial class LevelAuctionFilter : IAuctionFilter
{
    [UInt(32)] public uint Minimum { get; private set; }
    [UInt(32)] public uint Maximum { get; private set; }
}
