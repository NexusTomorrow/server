using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.ClientAuctionSellOrderSubmit, Direction.ClientToServer)]
public sealed partial class ClientAuctionSellOrderSubmit
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
    [UInt(64)] public ulong MinimumBid { get; private set; }
    [UInt(64)] public ulong BuyoutPrice { get; private set; }
}
