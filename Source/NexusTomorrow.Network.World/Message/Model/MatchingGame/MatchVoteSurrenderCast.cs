using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// The player's vote on a vote to surrender; the client sends it while a surrender vote runs.
/// </summary>
[WireMessage(GameMessageOpcode.Client0624, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchVoteSurrenderCast
{
    /// <summary>
    /// The player's vote on the surrender.
    /// </summary>
    [UInt(1)] public bool Vote { get; private set; }
}
