using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp
{
    [Message(GameMessageOpcode.ClientSetIgnoreDuelRequests)]
    public class ClientSetIgnoreDuelRequests : IReadable
    {
        public bool Ignore { get; private set; }

        public void Read(GamePacketReader reader)
        {
            Ignore = reader.ReadBit();
        }
    }
}
