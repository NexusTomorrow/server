using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerMovementControlRemove)]
    public class ServerMovementControlRemove : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
        }
    }
}
