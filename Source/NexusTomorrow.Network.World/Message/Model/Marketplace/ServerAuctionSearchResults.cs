using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[Message(GameMessageOpcode.ServerAuctionSearchResults)]
[WireMessage(GameMessageOpcode.ServerAuctionSearchResults, Direction.ServerToClient)]
public sealed partial class ServerAuctionSearchResults : IWritable
{
    [UInt(32)] public uint TotalResultCount { get; set; }
    [UInt(32)] public uint CurrentPage { get; set; }
    [Array(CountBits = 32)] public List<AuctionInfo> Auctions { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TotalResultCount);
        writer.Write(CurrentPage);
        writer.Write(Auctions.Count);
        Auctions.ForEach(auction => auction.Write(writer));
    }
}
