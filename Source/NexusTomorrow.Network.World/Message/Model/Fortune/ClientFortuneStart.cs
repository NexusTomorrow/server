using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune
{
    [Message(GameMessageOpcode.ClientFortuneStart)]
    public class ClientFortuneStart : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            // zero byte message
        }
    }
}
