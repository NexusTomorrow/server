using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Starts a vote to surrender the match; the client marks a surrender vote running and raises MatchVoteSurrenderBegin.
/// </summary>
[WireMessage(GameMessageOpcode.Server0621, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchVoteSurrenderBegin
{
}
