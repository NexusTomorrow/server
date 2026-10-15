using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility
{
    [Message(GameMessageOpcode.ClientPlayedRequest)]
    public class ClientPlayedReqeust : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            // packet has no payload
        }
    }
}
