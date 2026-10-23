using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Starts a vote to kick a player from the match; unless the player started it, the client keeps the target and raises
/// MatchVoteKickBegin with it.
/// </summary>
[WireMessage(GameMessageOpcode.Server0614, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MatchVoteKickBegin
{
    /// <summary>
    /// The player who started the vote, by realm-scoped character id.
    /// </summary>
    public RealmScopedId InitiatorId { get; set; } = new();

    /// <summary>
    /// The player the vote would kick, by realm-scoped character id; <see cref="MatchVoteKickCast.TargetId"/> sends it back.
    /// </summary>
    public RealmScopedId TargetId { get; set; } = new();
}
