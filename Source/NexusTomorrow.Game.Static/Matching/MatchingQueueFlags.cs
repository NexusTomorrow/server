namespace NexusTomorrow.Game.Static.Matching;

/// <summary>
/// The flags of a matching queue entry, as a queue request sends them and a joined entry carries them.
/// </summary>
[Flags]
public enum MatchingQueueFlags
{
    None = 0x00,

    /// <summary>
    /// The entry was joined as a group.
    /// </summary>
    GroupIsQueued = 0x04,

    Unknown20 = 0x20,

    /// <summary>
    /// The queue looks for other players.
    /// </summary>
    FindOthers = 0x80,

    /// <summary>
    /// The queue is a veteran queue.
    /// </summary>
    Veteran = 0x100,
}
