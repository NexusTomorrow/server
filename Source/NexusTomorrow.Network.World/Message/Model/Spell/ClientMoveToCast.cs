using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell
{
    [Message(GameMessageOpcode.ClientMoveToCast)]
    public class ClientMoveToCast : IReadable
    {
        public uint ClientSpellCastUniqueId { get; private set; }
        public ushort BagIndex { get; private set; }
        public Position MoveToPosition { get; private set; } = new Position();

        public void Read(GamePacketReader reader)
        {
            ClientSpellCastUniqueId = reader.ReadUInt();
            BagIndex = reader.ReadUShort();
            MoveToPosition.Read(reader);
        }
    }
}
