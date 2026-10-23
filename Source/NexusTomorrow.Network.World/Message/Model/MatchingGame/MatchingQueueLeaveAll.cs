using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Leaves every matching queue the player is in.
/// </summary>
[WireMessage(GameMessageOpcode.Client05B4, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchingQueueLeaveAll
{
}
