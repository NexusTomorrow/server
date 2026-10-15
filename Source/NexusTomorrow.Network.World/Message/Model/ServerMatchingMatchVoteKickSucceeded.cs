using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingMatchVoteKickSucceeded)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchVoteKickSucceeded, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchVoteKickSucceeded : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // Zero byte message
    }
}
