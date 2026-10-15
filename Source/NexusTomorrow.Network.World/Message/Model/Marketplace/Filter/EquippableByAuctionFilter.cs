using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(AuctionFilterType.EquippableBy)]
public sealed partial class EquippableByAuctionFilter : IAuctionFilter
{
    [UInt(8)] public Race RaceId { get; private set; }
    [Pad(6)] public Pad RaceIdHigh { get; }
    [UInt(8)] public Class ClassId { get; private set; }
    [Pad(6)] public Pad ClassIdHigh { get; }
    [UInt(14)] public Faction FactionId { get; private set; }
    [UInt(32)] public uint Level { get; private set; }
}
