using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

// MarketplaceLib.RequestOwnedItemAuctions()
[Message(GameMessageOpcode.ClientRequestOwnedItemAuctions)]
public class ClientRequestOwnedItemAuctions : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
