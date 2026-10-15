using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerPlayerEnteredWorld)]
[WireMessage(GameMessageOpcode.ServerPlayerEnteredWorld, Direction.ServerToClient)]
public sealed partial class ServerPlayerEnteredWorld : IWritable
{
    public void Write(GamePacketWriter writer)
    {
    }
}
