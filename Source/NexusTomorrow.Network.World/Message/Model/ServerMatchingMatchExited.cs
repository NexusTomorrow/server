using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerMatchingMatchExited)]
    public class ServerMatchingMatchExited : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
        }
    }
}
