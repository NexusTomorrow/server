using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientEntitySelect)]
    public class ClientEntitySelect : IReadable
    {
        public uint Guid { get; private set; }

        public void Read(GamePacketReader reader)
        {
            Guid = reader.ReadUInt();
        }
    }
}
