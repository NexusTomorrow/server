using System.Diagnostics.Metrics;

namespace NexusTomorrow.Maps;

/// <summary>
/// The maps' counters and histograms, published through <see cref="System.Diagnostics.Metrics"/>.
/// </summary>
public sealed class MapMetrics : IDisposable
{
    private readonly Meter _meter = new("NexusTomorrow.Maps");
    private readonly Counter<long> _ticks;
    private readonly Histogram<double> _duration;
    private readonly Histogram<long> _allocated;
    private readonly Counter<long> _skipped;
    private readonly Counter<long> _faults;
    private readonly Counter<long> _dropped;
    private readonly Counter<long> _admittedUnits;
    private readonly Counter<long> _droppedUnits;

    /// <summary>
    /// Create the instruments on a meter this object owns.
    /// </summary>
    public MapMetrics()
    {
        _admittedUnits = _meter.CreateCounter<long>("nexustomorrow.map.units_admitted", description: "Players created for a viewer that came into range");
        _droppedUnits = _meter.CreateCounter<long>("nexustomorrow.map.units_dropped", description: "Players destroyed for a viewer: out of range, hidden or gone");
        _ticks = _meter.CreateCounter<long>("nexustomorrow.map.ticks", description: "Map ticks completed");
        _duration = _meter.CreateHistogram<double>("nexustomorrow.map.tick.duration", unit: "ms", description: "How long a map tick ran");
        _allocated = _meter.CreateHistogram<long>("nexustomorrow.map.tick.allocated", unit: "By", description: "Bytes a map tick allocated");
        _skipped = _meter.CreateCounter<long>("nexustomorrow.map.ticks_skipped", description: "Map deadlines skipped because a tick ran past them");
        _faults = _meter.CreateCounter<long>("nexustomorrow.map.faults", description: "Map ticks, timers and message handlers that threw");
        _dropped = _meter.CreateCounter<long>("nexustomorrow.map.dropped", description: "Client messages a map read but did not act on");
    }

    /// <summary>
    /// Count one completed tick and record how long it ran and what it allocated.
    /// </summary>
    /// <param name="milliseconds">How long the tick ran.</param>
    /// <param name="allocatedBytes">The bytes the tick allocated on its thread.</param>
    /// <param name="world">The tag naming the world the map belongs to.</param>
    public void TickCompleted(double milliseconds, long allocatedBytes, KeyValuePair<string, object?> world)
    {
        _ticks.Add(1, world);
        _duration.Record(milliseconds, world);
        _allocated.Record(allocatedBytes, world);
    }

    /// <summary>
    /// Count deadlines skipped because a tick ran past them.
    /// </summary>
    /// <param name="count">How many deadlines were skipped.</param>
    public void TicksSkipped(long count) => _skipped.Add(count);

    /// <summary>
    /// Count one tick that threw, with no world tag.
    /// </summary>
    public void TickFailed() => _faults.Add(1);

    /// <summary>
    /// Count one fault inside a map: a timer or a message handler that threw.
    /// </summary>
    /// <param name="world">The tag naming the world the map belongs to.</param>
    public void Fault(KeyValuePair<string, object?> world) => _faults.Add(1, world);

    /// <summary>
    /// Count one client message a map read but did not act on.
    /// </summary>
    /// <param name="world">The tag naming the world the map belongs to.</param>
    public void Dropped(KeyValuePair<string, object?> world) => _dropped.Add(1, world);

    /// <summary>
    /// Count one player created for a viewer.
    /// </summary>
    /// <param name="world">The tag naming the world the map belongs to.</param>
    public void UnitAdmitted(KeyValuePair<string, object?> world) => _admittedUnits.Add(1, world);

    /// <summary>
    /// Count one player destroyed for a viewer.
    /// </summary>
    /// <param name="world">The tag naming the world the map belongs to.</param>
    public void UnitDropped(KeyValuePair<string, object?> world) => _droppedUnits.Add(1, world);

    /// <inheritdoc/>
    public void Dispose() => _meter.Dispose();
}
