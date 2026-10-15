using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerMatchingMatchFinished)]
    public class ServerMatchingMatchFinished : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
        }
    }
}
