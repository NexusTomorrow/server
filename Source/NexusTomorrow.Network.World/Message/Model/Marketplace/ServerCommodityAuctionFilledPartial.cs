using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires whenever a buy or sell order on the Commodities Exchange is partially filled by another player.
[WireMessage(GameMessageOpcode.ServerCommodityAuctionFilledPartial, Direction.ServerToClient)]
public sealed partial class ServerCommodityAuctionFilledPartial
{
    public CommodityOrder OrderFilled { get; set; } = new();
    [UInt(32)] public AuctionEventType Type { get; set; }
}
