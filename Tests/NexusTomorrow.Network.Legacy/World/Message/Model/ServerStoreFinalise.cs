using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerStoreFinalize)]
public class ServerStoreFinalise : IWritable
{
    public void Write(GamePacketWriter writer)
    {
    }
}
