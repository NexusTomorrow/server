using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
public sealed partial class AuctionFilter : IAuctionFilter
{
    [UInt(3)] public AuctionFilterType Type { get; private set; }
    [Union(nameof(Type))] public IAuctionFilter Filter { get; private set; }
}
