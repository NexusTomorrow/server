using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientEnteredWorld)]
    public class ClientEnteredWorld : IReadable
    {
        public ushort WorldZoneId { get; private set; } // 15

        public void Read(GamePacketReader reader)
        {
            WorldZoneId = reader.ReadUShort(15u);
        }
    }
}
