using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing
{
    [Message(GameMessageOpcode.ClientHousingAddItemToCrate)]
    public class ClientHousingAddItemToCrate : IReadable
    {
        public ulong ItemGuid { get; private set; }

        public void Read(GamePacketReader reader)
        {
            ItemGuid = reader.ReadULong();
        }
    }
}
