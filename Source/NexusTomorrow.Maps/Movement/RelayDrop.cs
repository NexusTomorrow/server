namespace NexusTomorrow.Maps.Movement;

/// <summary>
/// Why a map dropped a movement batch instead of relaying it.
/// </summary>
public enum RelayDrop
{
    /// <summary>
    /// Older than the last batch relayed for the mover, outside a reset.
    /// </summary>
    Older,

    /// <summary>
    /// More commands than <c>UnitMovement</c> carries (31).
    /// </summary>
    TooLong,

    /// <summary>
    /// The mover's queue was full until the next flush (16 batches).
    /// </summary>
    RingFull,

    /// <summary>
    /// The send gate or the codec refused the relay.
    /// </summary>
    Refused,
}
