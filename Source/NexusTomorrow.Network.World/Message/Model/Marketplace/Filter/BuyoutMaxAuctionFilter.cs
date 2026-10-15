using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(AuctionFilterType.BuyoutMax)]
public sealed partial class BuyoutMaxAuctionFilter : IAuctionFilter
{
    [UInt(64)] public ulong BuyoutPrice { get; private set; }

    public void Read(GamePacketReader reader)
    {
        BuyoutPrice = reader.ReadULong();
    }
}
