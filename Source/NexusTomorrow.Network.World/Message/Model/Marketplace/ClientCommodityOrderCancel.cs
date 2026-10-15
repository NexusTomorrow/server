using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[Message(GameMessageOpcode.ClientCommodityOrderCancel)]
[WireMessage(GameMessageOpcode.ClientCommodityOrderCancel, Direction.ClientToServer)]
public sealed partial class ClientCommodityOrderCancel : IReadable
{
    [UInt(64)] public ulong CommodityOrderId { get; private set; }
    [UInt(18)] public uint Item2Id { get; private set; }
    [UInt(1)] public bool IsBuyOrder { get; private set; }

    public void Read(GamePacketReader reader)
    {
        CommodityOrderId = reader.ReadULong();
        Item2Id = reader.ReadUInt(18u);
        IsBuyOrder = reader.ReadBit();
    }
}
