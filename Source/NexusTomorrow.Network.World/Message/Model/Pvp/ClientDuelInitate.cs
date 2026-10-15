using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp
{
    // No target information is sent. Relies on the target sent in ClientEntitySelect (0x185)
    [Message(GameMessageOpcode.ClientDuelInitate)]
    public class ClientDuelInitate : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            // Zero byte message
        }
    }
}
