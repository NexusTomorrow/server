using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.ServerAuctionSearchResults, Direction.ServerToClient)]
public sealed partial class ServerAuctionSearchResults
{
    [UInt(32)] public uint TotalResultCount { get; set; }
    [UInt(32)] public uint CurrentPage { get; set; }
    [Array(CountBits = 32)] public List<AuctionInfo> Auctions { get; set; } = [];
}
