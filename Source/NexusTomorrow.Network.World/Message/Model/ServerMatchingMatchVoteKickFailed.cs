using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Triggers MatchVoteKickEnd lua event on client
[Message(GameMessageOpcode.ServerMatchingMatchVoteKickFailed)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchVoteKickFailed, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchVoteKickFailed : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // Zero byte message
    }
}
