using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Triggers MatchVoteKickEnd lua event on client
[WireMessage(GameMessageOpcode.ServerMatchingMatchVoteKickFailed, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchVoteKickFailed
{
}
