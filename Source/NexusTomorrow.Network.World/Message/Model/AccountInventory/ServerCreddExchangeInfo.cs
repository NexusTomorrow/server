using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerCreddExchangeInfo, Direction.ServerToClient)]
public sealed partial class ServerCreddExchangeInfo
{
    [WireStruct]
    public sealed partial class CreddListing
    {
        [UInt(64)] public ulong ListingId { get; set; }
        [UInt(64)] public ulong Price { get; set; }
        [UInt(1)] public bool IsBuyOrder { get; set; }
        [UInt(64)] public ulong ListTime { get; set; } // a Win32 FILETIME
        [UInt(64)] public ulong ExpirationTime { get; set; } // a Win32 FILETIME
    }

    [UInt(32)] public uint BuyOrderCount { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] BuyOrderPrices { get; set; } = new ulong[3]; // average price of 1, 10, 50 purchase
    [UInt(32)] public uint SellOrderCount { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] SellOrderPrices { get; set; } = new ulong[3]; // average price of 1, 10, 50 purchase
    [Array(CountBits = 32)] public List<CreddListing> Listings { get; set; } = [];
}
