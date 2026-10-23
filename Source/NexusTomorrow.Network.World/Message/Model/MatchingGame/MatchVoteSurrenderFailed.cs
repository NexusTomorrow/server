using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Ends the vote to surrender as failed; the client marks no surrender vote running and raises MatchVoteSurrenderEnd.
/// </summary>
[WireMessage(GameMessageOpcode.Server061F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchVoteSurrenderFailed
{
}
