namespace NexusTomorrow.Owners.World;

/// <summary>
/// Where messages reach a map. Safe from any thread; every post returns false once the map has stopped.
/// </summary>
public interface IMapInbox
{
    /// <summary>
    /// The world the map runs.
    /// </summary>
    ushort WorldId { get; }

    /// <summary>
    /// Queue <paramref name="message"/> at normal priority; false once the map has stopped.
    /// </summary>
    bool Post(MapMessage message);

    /// <summary>
    /// Queue <paramref name="message"/> at high priority, which every tick drains in full before the normal queue; false once the
    /// map has stopped.
    /// </summary>
    bool PostHigh(MapMessage message);
}
