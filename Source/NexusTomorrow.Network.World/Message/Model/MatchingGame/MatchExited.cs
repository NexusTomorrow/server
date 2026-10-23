using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Tells the client the player is out of the match's instance; the client raises MatchExited.
/// </summary>
[WireMessage(GameMessageOpcode.Server05C0, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchExited
{
}
