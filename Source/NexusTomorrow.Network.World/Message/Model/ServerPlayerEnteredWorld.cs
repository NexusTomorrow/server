using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerPlayerEnteredWorld)]
    public class ServerPlayerEnteredWorld : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
        }
    }
}
