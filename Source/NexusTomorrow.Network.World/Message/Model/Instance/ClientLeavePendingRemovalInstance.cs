using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

// Can only be sent if a ServerPendingWorldRemoval (0x0689) has been sent and is still active
[Message(GameMessageOpcode.ClientLeavePendingRemovalInstance)]
[WireMessage(GameMessageOpcode.ClientLeavePendingRemovalInstance, Direction.ClientToServer)]
public sealed partial class ClientLeavePendingRemovalInstance : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
