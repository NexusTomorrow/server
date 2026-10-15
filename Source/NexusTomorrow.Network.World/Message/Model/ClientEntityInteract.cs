using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientEntityInteract)]
    public class ClientEntityInteract : IReadable
    {
        public uint UnitId { get; private set; } // Interaction unit
        public InteractionType Type { get; private set; }

        public void Read(GamePacketReader reader)
        {
            UnitId  = reader.ReadUInt();
            Type = reader.ReadEnum<InteractionType>(7);
        }
    }
}
