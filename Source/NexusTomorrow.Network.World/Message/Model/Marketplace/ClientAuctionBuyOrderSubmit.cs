using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[Message(GameMessageOpcode.ClientAuctionBuyOrderSubmit)]
[WireMessage(GameMessageOpcode.ClientAuctionBuyOrderSubmit, Direction.ClientToServer)]
public sealed partial class ClientAuctionBuyOrderSubmit : IReadable
{
    [UInt(18)] public uint Item2Id { get; private set; }
    [UInt(64)] public ulong AuctionId { get; private set; }
    [UInt(64)] public ulong AmountOffered { get; private set; } // is a buyout if AmountOffered == auction's buyout price

    public void Read(GamePacketReader reader)
    {
        Item2Id = reader.ReadUInt(18u);
        AuctionId = reader.ReadULong();
        AmountOffered = reader.ReadULong();
    }
}
