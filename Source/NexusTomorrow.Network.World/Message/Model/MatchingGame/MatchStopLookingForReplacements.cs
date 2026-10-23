using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Asks the match to stop looking for replacements; the client sends it from the group leader while the match is looking.
/// </summary>
[WireMessage(GameMessageOpcode.Client0602, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchStopLookingForReplacements
{
}
