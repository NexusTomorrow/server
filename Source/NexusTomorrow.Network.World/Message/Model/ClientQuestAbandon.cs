using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientQuestAbandon)]
    public class ClientQuestAbandon : IReadable
    {
        public ushort QuestId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            QuestId = reader.ReadUShort(15u);
        }
    }
}
