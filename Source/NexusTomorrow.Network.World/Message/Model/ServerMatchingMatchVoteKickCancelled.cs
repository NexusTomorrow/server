using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Triggers MatchVoteKickEnd lua event on client
[Message(GameMessageOpcode.ServerMatchingMatchVoteKickCancelled)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchVoteKickCancelled, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchVoteKickCancelled : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // Zero byte message
    }
}
