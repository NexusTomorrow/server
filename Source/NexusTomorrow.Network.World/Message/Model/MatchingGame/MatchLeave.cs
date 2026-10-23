using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Leaves the match; MatchingGameLib.LeaveGame sends it while the player is in one.
/// </summary>
[WireMessage(GameMessageOpcode.Client05DA, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchLeave
{
}
