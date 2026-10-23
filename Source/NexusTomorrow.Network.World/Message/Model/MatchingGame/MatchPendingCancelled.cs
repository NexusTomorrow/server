using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Cancels the pending match; the client raises MatchingCancelPendingGame.
/// </summary>
[WireMessage(GameMessageOpcode.Server05BC, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchPendingCancelled
{
}
