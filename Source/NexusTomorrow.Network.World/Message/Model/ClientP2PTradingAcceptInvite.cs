using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientP2PTradingAcceptInvite)]
    public class ClientP2PTradingAcceptInvite : IReadable
    {
        // Zero byte message
        public void Read(GamePacketReader reader)
        {
        }
    }
}
