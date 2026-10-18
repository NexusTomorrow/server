namespace NexusTomorrow.Maps.Scheduling;

/// <summary>
/// Runs maps at their tick periods; every member is safe from any thread.
/// </summary>
public interface IMapScheduler : IAsyncDisposable
{
    /// <summary>
    /// Start the pool threads.
    /// </summary>
    void Start();

    /// <summary>
    /// Run a map at its period from a first deadline within one period of now.
    /// </summary>
    /// <param name="map">The map to tick.</param>
    /// <param name="placement">Where its ticks run.</param>
    void Register(ITickable map, MapPlacement placement);

    /// <summary>
    /// Stop ticking a map.
    /// </summary>
    /// <param name="map">The map to stop.</param>
    /// <returns>A task that completes once the map can never tick again.</returns>
    Task UnregisterAsync(ITickable map);
}
