using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientResurrectRequest)]
    public class ClientResurrectRequest : IReadable
    {
        public void Read(GamePacketReader reader)
        {
        }
    }
}
