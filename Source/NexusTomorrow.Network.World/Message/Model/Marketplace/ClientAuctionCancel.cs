using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.ClientAuctionCancel, Direction.ClientToServer)]
public sealed partial class ClientAuctionCancel
{
    [UInt(64)] public ulong AuctionId { get; private set; }
    [UInt(18)] public uint Item2Id { get; private set; }
}
