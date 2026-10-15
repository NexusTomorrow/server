using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[Message(GameMessageOpcode.ServerGenericMapNode)]
[WireMessage(GameMessageOpcode.ServerGenericMapNode, Direction.ServerToClient)]
public sealed partial class ServerGenericMapNode : IWritable
{
    public GenericMapNode Node { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        Node.Write(writer);
    }
}
