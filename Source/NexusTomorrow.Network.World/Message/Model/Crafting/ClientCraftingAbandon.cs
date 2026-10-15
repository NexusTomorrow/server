using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting
{
    [Message(GameMessageOpcode.ClientCraftingAbandon)]
    public class ClientCraftingAbandon : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            // zero byte message
        }
    }
}
