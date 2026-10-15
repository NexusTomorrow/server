using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerCreddExchangeInfo)]
[WireMessage(GameMessageOpcode.ServerCreddExchangeInfo, Direction.ServerToClient)]
public sealed partial class ServerCreddExchangeInfo : IWritable
{
    [WireStruct]
    public sealed partial class CreddListing : IWritable
    {
        [UInt(64)] public ulong ListingId { get; set; }
        [UInt(64)] public ulong Price { get; set; }
        [UInt(1)] public bool IsBuyOrder { get; set; }
        [UInt(64)] public ulong ListTime { get; set; } // a Win32 FILETIME
        [UInt(64)] public ulong ExpirationTime { get; set; } // a Win32 FILETIME

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ListingId);
            writer.Write(Price);
            writer.Write(IsBuyOrder);
            writer.Write(ListTime);
            writer.Write(ExpirationTime);
        }
    }

    [UInt(32)] public uint BuyOrderCount { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] BuyOrderPrices { get; set; } = new ulong[3]; // average price of 1, 10, 50 purchase
    [UInt(32)] public uint SellOrderCount { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] SellOrderPrices { get; set; } = new ulong[3]; // average price of 1, 10, 50 purchase
    [Array(CountBits = 32)] public List<CreddListing> Listings { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(BuyOrderCount);
        for (int i = 0; i < BuyOrderPrices.Length; i++)
        {
            writer.Write(BuyOrderPrices[i]);
        }

        writer.Write(SellOrderCount);

        for (int i = 0; i < SellOrderPrices.Length; i++)
        {
            writer.Write(SellOrderPrices[i]);
        }

        writer.Write(Listings.Count);
        Listings.ForEach(listing => listing.Write(writer));
    }
}
