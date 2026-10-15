using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireStruct]
public sealed partial class AuctionInfo : IReadable, IWritable
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

    public void Read(GamePacketReader reader)
    {
        AuctionId = reader.ReadULong();
        OwnerCharacterId = reader.ReadULong();
        MinimumBid = reader.ReadULong();
        BuyoutPrice = reader.ReadULong();
        CurrentBid = reader.ReadULong();
        TopBidderCharacterId = reader.ReadULong();
        ExpirationTime = reader.ReadULong();
        Item2Id = reader.ReadUInt(18u);
        Quantity = reader.ReadUInt();
        WorldRequirement_Item2Id = reader.ReadUInt(18u);

        uint count = reader.ReadUInt(3u);
        for (uint i = 0; i < count; i++)
        {
            UnknownArray.Add(reader.ReadUInt());
        }

        CircuitData = reader.ReadULong();
        GlyphData = reader.ReadUInt();
        ThresholdData = reader.ReadULong();
        Unknown2 = reader.ReadUInt();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AuctionId);
        writer.Write(OwnerCharacterId);
        writer.Write(MinimumBid);
        writer.Write(BuyoutPrice);
        writer.Write(CurrentBid);
        writer.Write(TopBidderCharacterId);
        writer.Write(ExpirationTime);
        writer.Write(Item2Id, 18u);
        writer.Write(Quantity);
        writer.Write(WorldRequirement_Item2Id, 18u);

        writer.Write(UnknownArray.Count, 3u);
        UnknownArray.ForEach(unknown => writer.Write(unknown));

        writer.Write(CircuitData);
        writer.Write(GlyphData);
        writer.Write(ThresholdData);
        writer.Write(Unknown2);
    }
}
