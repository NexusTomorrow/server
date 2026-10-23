using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Starts a vote to surrender the match.
/// </summary>
/// <remarks>
/// The client sends it while no surrender vote runs and the active match type is a dungeon, an adventure, a warplot or a shiphand,
/// unless a surrender cooldown <see cref="MatchOperationResult"/> set is still running.
/// </remarks>
[WireMessage(GameMessageOpcode.Client05D3, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchVoteSurrenderInitiate
{
}
