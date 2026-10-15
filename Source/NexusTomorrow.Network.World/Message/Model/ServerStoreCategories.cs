using NexusTomorrow.Game.Static.Storefront;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerStoreCategories)]
[WireMessage(GameMessageOpcode.ServerStoreCategories, Direction.ServerToClient)]
public sealed partial class ServerStoreCategories : IWritable
{
    [WireStruct]
    public sealed partial class StoreCategory : IWritable
    {
        [WideString] public string Name { get; set; }
        [WideString] public string Description { get; set; }
        [UInt(32)] public uint CategoryId { get; set; } // Id of this category
        [UInt(32)] public uint ParentCategoryId { get; set; }
        [UInt(32)] public uint Index { get; set; } // Seems to be indexed from 1
        [UInt(1)] public bool Visible { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteStringWide(Name);
            writer.WriteStringWide(Description);
            writer.Write(CategoryId);
            writer.Write(ParentCategoryId);
            writer.Write(Index);
            writer.Write(Visible);
        }
    }

    [WireStruct]
    public sealed partial class CurrencyPackage : IWritable
    {
        [UInt(32)] public uint Id { get; set; }
        [WideString] public string Name { get; set; }
        [UInt(32)] public uint Count { get; set; }
        [Float32] public float Price { get; set; }
        [UInt(32)] public uint Unknown9 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Id);
            writer.WriteStringWide(Name);
            writer.Write(Count);
            writer.Write(Price);
            writer.Write(Unknown9);
        }
    }

    [Array(CountBits = 32)] public List<StoreCategory> StoreCategories { get; set; } = [];
    [UInt(3)] public RealCurrency RealCurrency { get; set; }
    [Array(CountBits = 32)] public List<CurrencyPackage> CurrencyPackages { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(StoreCategories.Count);
        StoreCategories.ForEach(e => e.Write(writer));

        writer.Write(RealCurrency, 3);

        writer.Write(CurrencyPackages.Count);
        CurrencyPackages.ForEach(e => e.Write(writer));
    }
}
