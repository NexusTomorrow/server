using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[Message(GameMessageOpcode.ServerGenericMapNode)]
public class ServerGenericMapNode : IWritable
{
    public GenericMapNode Node { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        Node.Write(writer);
    }
}
