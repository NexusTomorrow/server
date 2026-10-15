using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing
{
    [Message(GameMessageOpcode.ClientHousingNeighbourInviteResponse)]
    public class ClientHousingNeighbourInviteResponse : IReadable
    {
        public bool Accept { get; private set; }

        public void Read(GamePacketReader reader)
        {
            Accept = reader.ReadBit();
        }
    }
}
