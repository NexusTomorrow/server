using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientItemDelete)]
    public class ClientItemDelete : IReadable
    {
        public ItemLocation From { get; } = new();

        public void Read(GamePacketReader reader)
        {
            From.Read(reader);
        }
    }
}
