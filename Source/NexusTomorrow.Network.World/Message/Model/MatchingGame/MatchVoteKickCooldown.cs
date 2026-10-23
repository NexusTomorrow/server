using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Reports a vote to kick held back by a cooldown.
/// </summary>
/// <remarks>
/// On <see cref="MatchingQueueResult.PersonalKickCooldown"/> or <see cref="MatchingQueueResult.GlobalKickCooldown"/> the client keeps the
/// result and prints the time left in the matching text.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0616, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MatchVoteKickCooldown
{
    /// <summary>
    /// The result.
    /// </summary>
    [UInt(6)] public MatchingQueueResult Result { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }
}
