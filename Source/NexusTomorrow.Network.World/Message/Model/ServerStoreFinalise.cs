using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerStoreFinalize)]
[WireMessage(GameMessageOpcode.ServerStoreFinalize, Direction.ServerToClient)]
public sealed partial class ServerStoreFinalise : IWritable
{
    public void Write(GamePacketWriter writer)
    {
    }
}
