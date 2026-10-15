using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientResurrectAccept)]
    public class ClientResurrectAccept : IReadable
    {
        public uint UnitId { get; private set; }
        public ResurrectionType RezType { get; private set; }

        public void Read(GamePacketReader reader)
        {
            UnitId  = reader.ReadUInt();
            RezType = reader.ReadEnum<ResurrectionType>(32u);
        }
    }
}
