using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Ends the vote to kick as passed; the client clears the vote's target and raises MatchVoteKickEnd.
/// </summary>
[WireMessage(GameMessageOpcode.Server061A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchVoteKickSucceeded
{
}
