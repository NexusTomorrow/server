using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerMatchingMatchReadyCancel)]
    public class ServerMatchingMatchReadyCancel : IWritable
    {
        // Sent when someone declines an invitation and the group is put back in the queue.
        public void Write(GamePacketWriter writer)
        {
            // deliberately empty
        }
    }
}
