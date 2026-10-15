using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientPremiumLockboxKeyRequest)]
public class ClientPremiumLockboxKeyRequest : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
