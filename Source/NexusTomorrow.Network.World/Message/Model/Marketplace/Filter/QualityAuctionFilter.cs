using NexusTomorrow.Game.Static.Item;
using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(AuctionFilterType.Quality)]
public sealed partial class QualityAuctionFilter : IAuctionFilter
{
    [UInt(4)] public Quality Minimum { get; private set; }
    [UInt(4)] public Quality Maximum { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Minimum = reader.ReadEnum<Quality>(4u);
        Maximum = reader.ReadEnum<Quality>(4u);
    }
}
