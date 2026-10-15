using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// Fires whenever the player attempts to post a buy or sell order on the commodities exchange.
// Informs the player if the order was successfully placed or an error was thrown.
[Message(GameMessageOpcode.ServerCommodityOrderResult)]
[WireMessage(GameMessageOpcode.ServerCommodityOrderResult, Direction.ServerToClient)]
public sealed partial class ServerCommodityOrderResult : IWritable
{
    [UInt(8)] public GenericError Result { get; set; }
    public CommodityOrder OrderPosted { get; set; }
    [UInt(64)] public ulong CostToPostOrder { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 8u);
        OrderPosted.Write(writer);
        writer.Write(CostToPostOrder);
    }
}
