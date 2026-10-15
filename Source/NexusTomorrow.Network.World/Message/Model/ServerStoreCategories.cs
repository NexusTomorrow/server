using NexusTomorrow.Game.Static.Storefront;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerStoreCategories, Direction.ServerToClient)]
public sealed partial class ServerStoreCategories
{
    [WireStruct]
    public sealed partial class StoreCategory
    {
        [WideString] public string Name { get; set; }
        [WideString] public string Description { get; set; }
        [UInt(32)] public uint CategoryId { get; set; } // Id of this category
        [UInt(32)] public uint ParentCategoryId { get; set; }
        [UInt(32)] public uint Index { get; set; } // Seems to be indexed from 1
        [UInt(1)] public bool Visible { get; set; }
    }

    [WireStruct]
    public sealed partial class CurrencyPackage
    {
        [UInt(32)] public uint Id { get; set; }
        [WideString] public string Name { get; set; }
        [UInt(32)] public uint Count { get; set; }
        [Float32] public float Price { get; set; }
        [UInt(32)] public uint Unknown9 { get; set; }
    }

    [Array(CountBits = 32)] public List<StoreCategory> StoreCategories { get; set; } = [];
    [UInt(3)] public RealCurrency RealCurrency { get; set; }
    [Array(CountBits = 32)] public List<CurrencyPackage> CurrencyPackages { get; set; } = [];
}
