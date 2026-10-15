using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientBuybackItemFromVendor)]
    public class ClientBuybackItemFromVendor : IReadable
    {
        public uint UniqueId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            UniqueId = reader.ReadUInt();
        }
    }
}
