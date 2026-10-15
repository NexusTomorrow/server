using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[Message(GameMessageOpcode.ServerAuctionCancelResult)]
[WireMessage(GameMessageOpcode.ServerAuctionCancelResult, Direction.ServerToClient)]
public sealed partial class ServerAuctionCancelResult : IWritable
{
    public AuctionInfo Auction { get; set; }
    [UInt(8)] public GenericError Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Auction.Write(writer);
        writer.Write(Result, 8u);
    }
}
