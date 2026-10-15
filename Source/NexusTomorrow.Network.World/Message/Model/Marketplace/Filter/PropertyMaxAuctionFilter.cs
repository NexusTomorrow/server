using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(AuctionFilterType.PropertyMax)]
public sealed partial class PropertyMaxAuctionFilter : IAuctionFilter
{
    [UInt(8)] public Property UnitProperty2Id { get; private set; }
    [Float32] public float Maximum { get; private set; }
}
