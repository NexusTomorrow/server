using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientQuestRetry)]
    public class ClientQuestRetry : IReadable
    {
        public ushort QuestId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            QuestId = reader.ReadUShort(15u);
        }
    }
}
