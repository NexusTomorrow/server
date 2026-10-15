using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireStruct]
public sealed partial class CommodityOrder
{
    [UInt(64)] public ulong CommodityOrderId { get; set; }
    [UInt(18)] public uint Item2Id { get; set; }
    [UInt(32)] public uint Quantity { get; set; }
    [UInt(64)] public ulong PricePerUnit { get; set; }
    [UInt(64)] public ulong Price { get; set; }
    [UInt(1)] public bool IsBuyOrder { get; set; }
    [UInt(1)] public bool ForceImmediate { get; set; } // Changes the CommodityOrder so it attempts to buy or sell its items for the best price when it is posted.
    [UInt(64)] public ulong ListTime { get; set; } // is the system _FILETIME struct
    [UInt(64)] public ulong ExpirationTime { get; set; } // is the system _FILETIME struct
}
