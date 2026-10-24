using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Starts a vote to kick a player from the match; unless <see cref="Unknown0"/> is the player's own realm-scoped id, the client keeps the
/// target and raises MatchVoteKickBegin with it.
/// </summary>
[WireMessage(GameMessageOpcode.Server0614, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MatchVoteKickBegin
{
    public RealmScopedId Unknown0 { get; set; } = new();

    /// <summary>
    /// The player the vote would kick, by realm-scoped character id; <see cref="MatchVoteKickCast.TargetId"/> sends it back.
    /// </summary>
    public RealmScopedId TargetId { get; set; } = new();
}
