using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[Message(GameMessageOpcode.ServerAuctionWon)]
[WireMessage(GameMessageOpcode.ServerAuctionWon, Direction.ServerToClient)]
public sealed partial class ServerAuctionWon : IWritable
{
    public AuctionInfo Auction { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Auction.Write(writer);
    }
}
