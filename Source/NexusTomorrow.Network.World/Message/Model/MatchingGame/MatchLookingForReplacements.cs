using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Tells the client the match is looking for replacements; the client marks it so and raises MatchLookingForReplacements.
/// </summary>
[WireMessage(GameMessageOpcode.Server05F2, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchLookingForReplacements
{
}
