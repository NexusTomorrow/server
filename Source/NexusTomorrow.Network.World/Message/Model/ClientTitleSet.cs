using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientTitleSet)]
    public class ClientTitleSet : IReadable 
    {
        public ushort TitleId { get; set; }

        public void Read(GamePacketReader reader)
        {
            TitleId = reader.ReadUShort(14u);
        }
    }
}
