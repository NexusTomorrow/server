using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Tells the client the match stopped looking for replacements; the client raises MatchStoppedLookingForReplacements.
/// </summary>
[WireMessage(GameMessageOpcode.Server0604, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchStoppedLookingForReplacements
{
}
