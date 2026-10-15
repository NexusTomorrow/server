using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientMatchingQueueLeaveAll)]
    public class ClientMatchingQueueLeaveAll : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            // deliberately empty
        }
    }
}
