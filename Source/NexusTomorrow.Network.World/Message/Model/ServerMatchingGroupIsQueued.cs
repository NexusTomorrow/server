using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingGroupIsQueued)]
[WireMessage(GameMessageOpcode.ServerMatchingGroupIsQueued, Direction.ServerToClient)]
public sealed partial class ServerMatchingGroupIsQueued : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // Zero byte message
    }
}
