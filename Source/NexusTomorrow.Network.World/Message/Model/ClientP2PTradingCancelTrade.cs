using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientP2PTradingCancelTrade)]
    public class ClientP2PTradingCancelTrade : IReadable
    {
        // Zero byte message
        public void Read(GamePacketReader reader)
        {
        }
    }
}
