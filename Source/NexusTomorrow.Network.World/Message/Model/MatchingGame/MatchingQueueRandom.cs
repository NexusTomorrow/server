using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Joins the random matching queue of a match type.
/// </summary>
[WireMessage(GameMessageOpcode.Client05F8, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class MatchingQueueRandom
{
    /// <summary>
    /// The queue's match type.
    /// </summary>
    [UInt(5)] public MatchType MatchType { get; private set; }

    /// <summary>
    /// The queue flags, as <see cref="MatchingQueueRequest.Flags"/> carries them.
    /// </summary>
    [UInt(32)] public uint Flags { get; private set; }

    /// <summary>
    /// The roles queued for, as a mask of <see cref="Role"/>. For <see cref="MatchType.Dungeon"/> and <see cref="MatchType.Adventure"/>
    /// the client sends at least one, and refuses with <see cref="MatchingQueueResult.Role"/> instead of sending none.
    /// </summary>
    [UInt(32)] public Role Roles { get; private set; }
}
