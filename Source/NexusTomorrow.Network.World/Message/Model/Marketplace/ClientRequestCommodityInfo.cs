using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// MarketplaceLib::RequestCommodityInfo()
[Message(GameMessageOpcode.ClientRequestCommodityInfo)]
[WireMessage(GameMessageOpcode.ClientRequestCommodityInfo, Direction.ClientToServer)]
public sealed partial class ClientRequestCommodityInfo : IReadable
{
    [UInt(18)] public uint Item2Id { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Item2Id = reader.ReadUInt(18u);
    }
}
