using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Sets which matching queues the player is in and the match type of the match they are in.
/// </summary>
/// <remarks>
/// The client applies <see cref="Unknown0"/> and <see cref="QueuedTypes"/> first, then stores <see cref="ActiveMatchType"/> and
/// <see cref="Unknown2"/>. It raises MatchLeft first only when the stored active match type is not <see cref="MatchType.None"/> and
/// differs from the new one.
/// </remarks>
[WireMessage(GameMessageOpcode.Server05E3, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MatchingQueueStatusChanged
{
    [UInt(4)] public byte Unknown0 { get; set; }

    /// <summary>
    /// The match type of the match the player is in, or <see cref="MatchType.None"/> for none; the client marks the queue entry of
    /// that type as in that match.
    /// </summary>
    [UInt(5)] public MatchType ActiveMatchType { get; set; }

    [UInt(5)] public byte Unknown2 { get; set; }

    /// <summary>
    /// The queues the player is in, one flag per <see cref="MatchType"/>, indexed by its value. The client adds an entry for each type
    /// whose flag is set, drops the entries of the others, and raises MatchingLeaveQueue with the types dropped.
    /// </summary>
    [FixedArray(16), UInt(1)] public bool[] QueuedTypes { get; set; } = new bool[16];
}
