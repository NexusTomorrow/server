using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Starts a vote to kick a player from the match; MatchingGameLib.InitiateVoteToKick sends it.
/// </summary>
[WireMessage(GameMessageOpcode.Client05D1, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchVoteKickInitiate
{
    /// <summary>
    /// The group member to kick, by realm-scoped character id; the client chooses the target among the group's members and never
    /// names the player themself.
    /// </summary>
    public RealmScopedId TargetId { get; private set; } = new();
}
