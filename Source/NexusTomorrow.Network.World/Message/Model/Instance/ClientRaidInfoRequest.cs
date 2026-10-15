using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance
{
    [Message(GameMessageOpcode.ClientRaidInfoRequest)]
    public class ClientRaidInfoRequest : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            // Zero byte message
        }
    }
}
