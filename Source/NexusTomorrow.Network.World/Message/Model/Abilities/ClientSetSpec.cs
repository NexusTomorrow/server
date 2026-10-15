using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities
{
    [Message(GameMessageOpcode.ClientSetSpec)]
    public class ClientSetSpec : IReadable
    {
        public byte SpecIndex { get; private set; }

        public void Read(GamePacketReader reader)
        {
            SpecIndex = reader.ReadByte(3u);
        }
    }
}
