using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[Message(GameMessageOpcode.ClientAuctionCancel)]
[WireMessage(GameMessageOpcode.ClientAuctionCancel, Direction.ClientToServer)]
public sealed partial class ClientAuctionCancel : IReadable
{
    [UInt(64)] public ulong AuctionId { get; private set; }
    [UInt(18)] public uint Item2Id { get; private set; }

    public void Read(GamePacketReader reader)
    {
        AuctionId = reader.ReadULong();
        Item2Id = reader.ReadUInt(18u);
    }
}
