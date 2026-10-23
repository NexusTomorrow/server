using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Marks the match finished; the client raises MatchFinished.
/// </summary>
[WireMessage(GameMessageOpcode.Server05BE, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchFinished
{
}
