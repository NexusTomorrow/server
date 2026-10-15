using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerStoreFinalise)]
    public class ServerStoreFinalise : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
        }
    }
}
