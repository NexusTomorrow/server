using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock
{
    [Message(GameMessageOpcode.ClientItemGenericUnlock)]
    public class ClientItemGenericUnlock : IReadable
    {
        public ItemLocation Location { get; } = new();

        public void Read(GamePacketReader reader)
        {
            Location.Read(reader);
        }
    }
}
