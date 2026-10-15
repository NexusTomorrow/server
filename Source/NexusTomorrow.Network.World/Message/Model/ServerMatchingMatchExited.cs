using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingMatchExited)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchExited, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchExited : IWritable
{
    public void Write(GamePacketWriter writer)
    {
    }
}
