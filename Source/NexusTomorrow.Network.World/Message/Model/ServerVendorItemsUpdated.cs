using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerVendorItemsUpdated)]
[WireMessage(GameMessageOpcode.ServerVendorItemsUpdated, Direction.ServerToClient)]
public sealed partial class ServerVendorItemsUpdated : IWritable
{
    [WireStruct]
    public sealed partial class Category : IWritable
    {
        [UInt(32)] public uint Index { get; set; }
        [UInt(32)] public uint LocalisedTextId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Index);
            writer.Write(LocalisedTextId);
        }
    }

    [WireStruct]
    public sealed partial class Item : IWritable
    {
        [WireStruct]
        public sealed partial class ItemExtraCost : IWritable
        {
            [UInt(3)] public ItemExtraCostType ExtraCostType { get; set; }
            [UInt(32)] public uint Quantity { get; set; }
            [UInt(32)] public uint ItemOrCurrencyId { get; set; }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(ExtraCostType, 3);
                writer.Write(Quantity);
                writer.Write(ItemOrCurrencyId);
            }
        }

        [UInt(32)] public uint Index { get; set; }
        [UInt(4)] public byte Unknown1 { get; set; }
        [UInt(32)] public uint ItemId { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(17)] public uint Unknown5 { get; set; }
        [UInt(32)] public uint Unknown6 { get; set; }
        [UInt(32)] public uint CategoryIndex { get; set; }
        [UInt(32)] public uint Unknown8 { get; set; }
        [UInt(64)] public ulong Unknown9 { get; set; }
        [UInt(32)] public uint UnknownA { get; set; }
        public ItemExtraCost ExtraCost1 { get; set; }
        public ItemExtraCost ExtraCost2 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Index);
            writer.Write(Unknown1, 4);
            writer.Write(ItemId);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5, 17);
            writer.Write(Unknown6);
            writer.Write(CategoryIndex);
            writer.Write(Unknown8);
            writer.Write(Unknown9);
            writer.Write(UnknownA);
            ExtraCost1.Write(writer);
            ExtraCost2.Write(writer);
        }
    }

    [UInt(32)] public uint Guid { get; set; }
    [Array(CountBits = 32)] public List<Category> Categories { get; } = [];
    [Array(CountBits = 32)] public List<Item> Items { get; } = [];
    [Float32] public float SellPriceMultiplier { get; set; }
    [Float32] public float BuyPriceMultiplier { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);

        writer.Write(Categories.Count);
        Categories.ForEach(c => c.Write(writer));
        writer.Write(Items.Count);
        Items.ForEach(i => i.Write(writer));

        writer.Write(SellPriceMultiplier);
        writer.Write(BuyPriceMultiplier);
        writer.Write(Unknown2);
        writer.Write(Unknown3);
        writer.Write(Unknown4);
    }
}
