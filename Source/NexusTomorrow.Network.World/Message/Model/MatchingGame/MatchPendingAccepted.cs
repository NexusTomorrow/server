using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Counts one more player of the pending match as accepted; the client raises MatchingGamePendingUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server05CC, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchPendingAccepted
{
    /// <summary>
    /// Whether the player who accepted is an ally (true) or an enemy (false); the client moves one of that side from yet to answer to
    /// accepted.
    /// </summary>
    [UInt(1)] public bool Ally { get; set; }
}
