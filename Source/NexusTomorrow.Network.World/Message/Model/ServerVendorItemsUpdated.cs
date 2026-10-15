using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerVendorItemsUpdated, Direction.ServerToClient)]
public sealed partial class ServerVendorItemsUpdated
{
    [WireStruct]
    public sealed partial class Category
    {
        [UInt(32)] public uint Index { get; set; }
        [UInt(32)] public uint LocalisedTextId { get; set; }
    }

    [WireStruct]
    public sealed partial class Item
    {
        [WireStruct]
        public sealed partial class ItemExtraCost
        {
            [UInt(3)] public ItemExtraCostType ExtraCostType { get; set; }
            [UInt(32)] public uint Quantity { get; set; }
            [UInt(32)] public uint ItemOrCurrencyId { get; set; }
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
    }

    [UInt(32)] public uint Guid { get; set; }
    [Array(CountBits = 32)] public List<Category> Categories { get; } = [];
    [Array(CountBits = 32)] public List<Item> Items { get; } = [];
    [Float32] public float SellPriceMultiplier { get; set; }
    [Float32] public float BuyPriceMultiplier { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }
}
