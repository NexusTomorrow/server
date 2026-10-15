using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing
{
    [Message(GameMessageOpcode.ClientHousingRequestRandomResidenceList)]
    public class ClientHousingRequestRandomResidenceList : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            // Zero byte message
        }
    }
}
