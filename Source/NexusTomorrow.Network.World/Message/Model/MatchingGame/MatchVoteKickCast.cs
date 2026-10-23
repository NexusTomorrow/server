using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// The player's vote on a vote to kick; the client clears the vote's target once it sends it.
/// </summary>
[WireMessage(GameMessageOpcode.Client0617, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchVoteKickCast
{
    /// <summary>
    /// The player the vote would kick, as <see cref="MatchVoteKickBegin.TargetId"/> gave it.
    /// </summary>
    public RealmScopedId TargetId { get; private set; } = new();

    /// <summary>
    /// The player's vote on the kick.
    /// </summary>
    [UInt(1)] public bool Vote { get; private set; }
}
