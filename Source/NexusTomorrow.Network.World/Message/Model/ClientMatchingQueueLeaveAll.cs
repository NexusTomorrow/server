using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingQueueLeaveAll)]
[WireMessage(GameMessageOpcode.ClientMatchingQueueLeaveAll, Direction.ClientToServer)]
public sealed partial class ClientMatchingQueueLeaveAll : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // deliberately empty
    }
}
