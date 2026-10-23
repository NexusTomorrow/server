using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Sets which matching queues the player is in, the match type of the match they are in, and that of the role check under way.
/// </summary>
/// <remarks>
/// The client applies <see cref="State"/> and <see cref="QueuedTypes"/> first, then stores <see cref="ActiveMatchType"/> and
/// <see cref="RoleCheckMatchType"/>. It raises MatchLeft first only when the stored active match type is not
/// <see cref="MatchType.None"/> and differs from the new one.
/// </remarks>
[WireMessage(GameMessageOpcode.Server05E3, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MatchingQueueStatusChanged
{
    /// <summary>
    /// The queue state. On 0, or any value above 9, the client leaves the match and cancels a role check; on 1 and 6 it cancels a role
    /// check; on any other value it does nothing.
    /// </summary>
    [UInt(4)] public byte State { get; set; }

    /// <summary>
    /// The match type of the match the player is in, or <see cref="MatchType.None"/> for none; the client marks the queue entry of
    /// that type as in that match.
    /// </summary>
    [UInt(5)] public MatchType ActiveMatchType { get; set; }

    /// <summary>
    /// The match type of the role check under way, or <see cref="MatchType.None"/> for none;
    /// <see cref="MatchingRoleCheckResponse.MatchType"/> sends it back.
    /// </summary>
    [UInt(5)] public MatchType RoleCheckMatchType { get; set; }

    /// <summary>
    /// The queues the player is in, one flag per <see cref="MatchType"/>, indexed by its value. The client adds an entry for each type
    /// whose flag is set, drops the entries of the others, and raises MatchingLeaveQueue with the types dropped.
    /// </summary>
    [FixedArray(16), UInt(1)] public bool[] QueuedTypes { get; set; } = new bool[16];
}
