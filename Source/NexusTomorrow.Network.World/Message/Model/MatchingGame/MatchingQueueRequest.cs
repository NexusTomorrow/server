using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// A matching queue to join: its match type, the maps and game type chosen, and the queue flags.
/// </summary>
[WireStruct]
public sealed partial class MatchingQueueRequest
{
    /// <summary>
    /// The queue's match type.
    /// </summary>
    [UInt(5)] public MatchType MatchType { get; set; }

    /// <summary>
    /// The maps queued for, as MatchingGameMap ids.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public List<uint> GameMapIds { get; set; } = [];

    /// <summary>
    /// The game type, as a MatchingGameType id.
    /// </summary>
    [UInt(14)] public ushort GameTypeId { get; set; }

    /// <summary>
    /// The queue flags. The client sets <see cref="MatchingQueueFlags.FindOthers"/> when it looks for others and
    /// <see cref="MatchingQueueFlags.Veteran"/> for a veteran queue, and keeps each only where the match type allows it. On an entry it
    /// receives in <see cref="MatchingQueueJoined"/> it adds <see cref="MatchingQueueFlags.GroupIsQueued"/> when
    /// <see cref="MatchingQueueJoined.MatchingQueueEntry.AsGroup"/> is set, and compares <see cref="MatchingQueueFlags.Veteran"/> with the
    /// entry's own veteran flag.
    /// </summary>
    [UInt(32)] public MatchingQueueFlags Flags { get; set; }
}
