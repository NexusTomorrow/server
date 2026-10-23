using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// The player's answer to a ready match.
/// </summary>
[WireMessage(GameMessageOpcode.Client05C8, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchReadyResponse
{
    /// <summary>
    /// Whether the player accepts the match (true) or declines it (false); on a refusal the client raises MatchingCancelPendingGame.
    /// </summary>
    [UInt(1)] public bool Accepted { get; private set; }
}
