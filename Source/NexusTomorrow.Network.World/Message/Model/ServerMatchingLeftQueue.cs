using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingLeftQueue)]
[WireMessage(GameMessageOpcode.ServerMatchingLeftQueue, Direction.ServerToClient)]
public sealed partial class ServerMatchingLeftQueue : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // Zero byte message
    }
}
