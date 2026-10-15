using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMovementControlRemove)]
[WireMessage(GameMessageOpcode.ServerMovementControlRemove, Direction.ServerToClient)]
public sealed partial class ServerMovementControlRemove : IWritable
{
    public void Write(GamePacketWriter writer)
    {
    }
}
