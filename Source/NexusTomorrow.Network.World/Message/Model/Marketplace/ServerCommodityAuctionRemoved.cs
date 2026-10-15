using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires whenever a buy or sell order is removed from the Commodities Exchange.
[Message(GameMessageOpcode.ServerCommodityAuctionRemoved)]
[WireMessage(GameMessageOpcode.ServerCommodityAuctionRemoved, Direction.ServerToClient)]
public sealed partial class ServerCommodityAuctionRemoved : IWritable
{
    public CommodityOrder OrderRemoved { get; set; } = new();
    [UInt(32)] public AuctionEventType Type { get; set; }

    public void Write(GamePacketWriter writer)
    {
        OrderRemoved.Write(writer);
        writer.Write(Type, 32u);
    }
}
