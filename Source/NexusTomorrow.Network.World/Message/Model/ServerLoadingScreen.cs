using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    // Seems be to used after a cinematic
    [Message(GameMessageOpcode.ServerLoadingScreen)]
    public class ServerLoadingScreen : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
            // zero byte message
        }
    }
}
