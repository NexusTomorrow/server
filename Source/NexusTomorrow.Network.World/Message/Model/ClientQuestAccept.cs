using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientQuestAccept)]
    public class ClientQuestAccept : IReadable
    {
        public ItemLocation ItemLocation { get; } = new();
        public ushort QuestId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            ItemLocation.Read(reader);
            QuestId = reader.ReadUShort(15u);
        }
    }
}
