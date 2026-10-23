using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Adds a matching queue entry, filed under <see cref="MatchingQueueEntry.MatchType"/>.
/// </summary>
/// <remarks>
/// For an entry already in its match the client stores the fields and does nothing else. Otherwise it also prints the joined text,
/// raises MatchingJoinQueue, ends any role check, and stops the player's cast of one spell a GameFormula row names.
/// </remarks>
[WireMessage(GameMessageOpcode.Server05DF, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchingQueueJoined
{
    /// <summary>
    /// The entry's match type, how it was joined, and its times.
    /// </summary>
    [WireStruct]
    public sealed partial class MatchingQueueEntry
    {
        /// <summary>
        /// The match type the entry is filed under.
        /// </summary>
        [UInt(5)] public MatchType MatchType { get; set; }

        /// <summary>
        /// Whether the entry was joined as a group; the client then sets <see cref="MatchingQueueFlags.GroupIsQueued"/> in the entry's
        /// flags.
        /// </summary>
        [UInt(1)] public bool AsGroup { get; set; }

        /// <summary>
        /// How long the entry has already waited, in milliseconds; the client counts its time in queue on from there.
        /// </summary>
        [UInt(32)] public uint TimeInQueueMs { get; set; }

        /// <summary>
        /// The queue's average wait, in milliseconds.
        /// </summary>
        [UInt(32)] public uint AverageWaitTimeMs { get; set; }
    }

    /// <summary>
    /// The queue joined: its maps, game type and flags. The client takes the entry's match type from <see cref="Entry"/>, not from this
    /// record.
    /// </summary>
    public MatchingQueueRequest Request { get; set; } = new();

    /// <summary>
    /// The entry's match type, how it was joined, and its times.
    /// </summary>
    public MatchingQueueEntry Entry { get; set; } = new();

    /// <summary>
    /// The roles queued for, as a mask of <see cref="Role"/>.
    /// </summary>
    [UInt(32)] public Role Roles { get; set; }
}
