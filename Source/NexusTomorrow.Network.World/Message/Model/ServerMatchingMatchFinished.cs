using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingMatchFinished)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchFinished, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchFinished : IWritable
{
    public void Write(GamePacketWriter writer)
    {
    }
}
