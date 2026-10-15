using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires whenever a buy or sell order on the Commodities Exchange is partially filled by another player.
[Message(GameMessageOpcode.ServerCommodityAuctionFilledPartial)]
[WireMessage(GameMessageOpcode.ServerCommodityAuctionFilledPartial, Direction.ServerToClient)]
public sealed partial class ServerCommodityAuctionFilledPartial : IWritable
{
    public CommodityOrder OrderFilled { get; set; } = new();
    [UInt(32)] public AuctionEventType Type { get; set; }

    public void Write(GamePacketWriter writer)
    {
        OrderFilled.Write(writer);
        writer.Write(Type, 32u);
    }
}
