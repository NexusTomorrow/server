using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientPremiumLockboxKeyRequest)]
[WireMessage(GameMessageOpcode.ClientPremiumLockboxKeyRequest, Direction.ClientToServer)]
public sealed partial class ClientPremiumLockboxKeyRequest : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
