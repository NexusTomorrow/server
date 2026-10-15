using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireStruct]
public sealed partial class AuctionInfo
{
    [UInt(64)] public ulong AuctionId { get; set; }
    [UInt(64)] public ulong OwnerCharacterId { get; set; }
    [UInt(64)] public ulong MinimumBid { get; set; }
    [UInt(64)] public ulong BuyoutPrice { get; set; }
    [UInt(64)] public ulong CurrentBid { get; set; }
    [UInt(64)] public ulong TopBidderCharacterId { get; set; }
    [UInt(64)] public ulong ExpirationTime { get; set; } // in seconds from now
    [UInt(18)] public uint Item2Id { get; set; }
    [UInt(32)] public uint Quantity { get; set; }
    [UInt(18)] public uint WorldRequirement_Item2Id { get; set; }
    [Array(CountBits = 3), UInt(32)] public List<uint> UnknownArray { get; set; } = [];
    [UInt(64)] public ulong CircuitData { get; set; }
    [UInt(32)] public uint GlyphData { get; set; }
    [UInt(64)] public ulong ThresholdData { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
