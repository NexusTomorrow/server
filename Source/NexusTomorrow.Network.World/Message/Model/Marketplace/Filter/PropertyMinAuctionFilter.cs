using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(AuctionFilterType.PropertyMin)]
public sealed partial class PropertyMinAuctionFilter : IAuctionFilter
{
    [UInt(8)] public Property UnitProperty2Id { get; private set; }
    [Float32] public float Minimum { get; private set; }

    public void Read(GamePacketReader reader)
    {
        UnitProperty2Id = reader.ReadEnum<Property>(8u);
        Minimum = reader.ReadSingle();
    }
}
