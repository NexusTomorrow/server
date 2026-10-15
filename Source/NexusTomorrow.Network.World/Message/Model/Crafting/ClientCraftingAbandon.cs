using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientCraftingAbandon)]
[WireMessage(GameMessageOpcode.ClientCraftingAbandon, Direction.ClientToServer)]
public sealed partial class ClientCraftingAbandon : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
