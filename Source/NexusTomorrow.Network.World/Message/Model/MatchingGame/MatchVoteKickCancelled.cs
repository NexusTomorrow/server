using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Ends the vote to kick as cancelled; the client clears the vote's target and raises MatchVoteKickEnd.
/// </summary>
[WireMessage(GameMessageOpcode.Server060D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchVoteKickCancelled
{
}
