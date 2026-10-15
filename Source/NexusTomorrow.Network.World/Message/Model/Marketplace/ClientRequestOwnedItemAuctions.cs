using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// MarketplaceLib.RequestOwnedItemAuctions()
[Message(GameMessageOpcode.ClientRequestOwnedItemAuctions)]
[WireMessage(GameMessageOpcode.ClientRequestOwnedItemAuctions, Direction.ClientToServer)]
public sealed partial class ClientRequestOwnedItemAuctions : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
