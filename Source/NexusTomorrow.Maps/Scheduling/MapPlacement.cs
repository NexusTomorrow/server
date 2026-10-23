namespace NexusTomorrow.Maps.Scheduling;

/// <summary>
/// Where a map's ticks run.
/// </summary>
public enum MapPlacement
{
    /// <summary>
    /// With the long tail of maps, on the pool's threads.
    /// </summary>
    Pool,

    /// <summary>
    /// On a thread of its own, while one is free in the budget.
    /// </summary>
    Dedicated,
}
