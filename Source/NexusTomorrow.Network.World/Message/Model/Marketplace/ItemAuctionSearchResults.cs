using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server0563, Direction.ServerToClient)]
public sealed partial class ItemAuctionSearchResults
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [Array(CountBits = 32)] public List<ItemAuction> Unknown2 { get; set; } = [];
}
