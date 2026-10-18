using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NexusTomorrow.Maps.Scheduling;

/// <summary>
/// Pool threads take the map whose deadline is earliest and has come, run one tick to completion and put it back; a dedicated
/// map runs alone on its thread. A map is held by one thread at a time, so its tick needs no lock. A deadline missed while a
/// tick ran is skipped, never replayed, and each map's first deadline is staggered across one tick (decisions.md D1, R2).
/// </summary>
public sealed class MapScheduler : IMapScheduler
{
    private const int IdleWaitMs = 100;

    private readonly MapOptions _options;
    private readonly TimeProvider _time;
    private readonly Random _random;
    private readonly MapMetrics _metrics;
    private readonly ILogger<MapScheduler> _logger;
    private readonly long _startTimestamp;

    // The lock guards the maps, the queue, the dedicated count, _started, the thread list and the Random.
    private readonly Lock _lock = new();
    private readonly Dictionary<ITickable, ScheduledMap> _maps = new(ReferenceEqualityComparer.Instance);
    private readonly DeadlineQueue _queue = new();
    private readonly List<TaskCompletionSource> _threads = [];
    private readonly AutoResetEvent _wake = new(false);
    private readonly ManualResetEvent _stop = new(false);
    private int _dedicatedInUse;
    private bool _started;

    // Read by every loop outside the lock; written once, under the lock, by DisposeAsync.
    private volatile bool _stopping;

    /// <summary>
    /// Create a scheduler; no thread runs until <see cref="Start"/> or a dedicated registration.
    /// </summary>
    /// <param name="options">The pool and dedicated thread counts.</param>
    /// <param name="time">The clock every deadline is read from.</param>
    /// <param name="random">Staggers first deadlines; used only under the scheduler's lock.</param>
    /// <param name="metrics">Counts skipped deadlines and failed ticks.</param>
    /// <param name="logger">Logs failed ticks and dedicated placements that fell back to the pool.</param>
    public MapScheduler(IOptions<MapOptions> options, TimeProvider time, Random random, MapMetrics metrics, ILogger<MapScheduler> logger)
    {
        _options = options.Value;
        _time = time;
        _random = random;
        _metrics = metrics;
        _logger = logger;
        _startTimestamp = time.GetTimestamp();
    }

    // Milliseconds elapsed on the injected clock since construction.
    internal long NowMs => (long)_time.GetElapsedTime(_startTimestamp).TotalMilliseconds;

    internal long DeadlineOf(ITickable map)
    {
        lock (_lock)
        {
            return _maps[map].DeadlineMs;
        }
    }

    /// <inheritdoc/>
    public void Start()
    {
        lock (_lock)
        {
            if (_stopping)
                throw new InvalidOperationException("The map scheduler is stopping.");
            if (_started)
                throw new InvalidOperationException("The map scheduler was already started.");

            _started = true;
            for (int i = 0; i < _options.PoolThreads; i++)
                StartThread($"map pool {i}", PoolLoop);
        }
    }

    /// <inheritdoc/>
    public void Register(ITickable map, MapPlacement placement)
    {
        bool fellBack = false;
        lock (_lock)
        {
            if (_stopping)
                throw new InvalidOperationException("The map scheduler is stopping.");
            if (_maps.ContainsKey(map))
                throw new InvalidOperationException($"Map {map.Name} is already registered.");

            var scheduled = new ScheduledMap(map, NowMs + _random.Next(map.TickMs));
            _maps.Add(map, scheduled);

            if (placement == MapPlacement.Dedicated && _dedicatedInUse < _options.DedicatedThreads)
            {
                _dedicatedInUse++;
                StartThread($"map {map.Name}", end => DedicatedLoop(scheduled, end));
            }
            else
            {
                fellBack = placement == MapPlacement.Dedicated;
                _queue.Add(scheduled);
                _wake.Set();
            }
        }

        if (fellBack)
            MapLog.NoDedicatedThread(_logger, map.Name);
    }

    /// <inheritdoc/>
    public Task UnregisterAsync(ITickable map)
    {
        lock (_lock)
        {
            if (!_maps.Remove(map, out ScheduledMap? scheduled))
                return Task.CompletedTask;

            scheduled.Removed = true;
            if (!scheduled.Running)
                scheduled.Unregistered.TrySetResult();

            return scheduled.Unregistered.Task;
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        TaskCompletionSource[] threads;
        lock (_lock)
        {
            if (_stopping)
                return;

            _stopping = true;
            threads = [.. _threads];
            foreach (ScheduledMap scheduled in _maps.Values)
                scheduled.Unregistered.TrySetResult();
        }

        _stop.Set();

        var ends = new Task[threads.Length];
        for (int i = 0; i < threads.Length; i++)
            ends[i] = threads[i].Task;

        await Task.WhenAll(ends);

        _wake.Dispose();
        _stop.Dispose();
    }

    // Called under the lock, with _stopping false, so the thread is in the snapshot disposal waits on.
    private void StartThread(string name, Action<TaskCompletionSource> body)
    {
        var end = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        _threads.Add(end);
        new Thread(() => body(end)) { IsBackground = true, Name = name }.Start();
    }

    private void PoolLoop(TaskCompletionSource end)
    {
        using IDeadlineWait wait = DeadlineWait.ForThisPlatform(_wake, _stop);
        try
        {
            while (!_stopping)
            {
                ScheduledMap? due;
                long waitMs;
                long nowMs;
                lock (_lock)
                {
                    nowMs = NowMs;
                    due = _queue.TakeDue(nowMs, out waitMs);
                    if (due is not null)
                        due.Running = true;
                }

                if (due is null)
                {
                    wait.Wait(TimeSpan.FromMilliseconds(waitMs < 0 ? IdleWaitMs : waitMs));
                    continue;
                }

                RunTick(due, nowMs);

                lock (_lock)
                {
                    due.Running = false;
                    if (due.Removed)
                    {
                        due.Unregistered.TrySetResult();
                    }
                    else
                    {
                        Reschedule(due);
                        _queue.Add(due);
                    }
                }
            }
        }
        finally
        {
            end.TrySetResult();
        }
    }

    private void DedicatedLoop(ScheduledMap scheduled, TaskCompletionSource end)
    {
        using IDeadlineWait wait = DeadlineWait.ForThisPlatform(_stop);
        try
        {
            while (!_stopping)
            {
                long nowMs;
                long waitMs;
                lock (_lock)
                {
                    if (scheduled.Removed)
                        break;

                    nowMs = NowMs;
                    waitMs = scheduled.DeadlineMs - nowMs;
                    if (waitMs <= 0)
                        scheduled.Running = true;
                }

                if (waitMs > 0)
                {
                    wait.Wait(TimeSpan.FromMilliseconds(waitMs));
                    continue;
                }

                RunTick(scheduled, nowMs);

                lock (_lock)
                {
                    scheduled.Running = false;
                    if (scheduled.Removed)
                    {
                        scheduled.Unregistered.TrySetResult();
                        break;
                    }

                    Reschedule(scheduled);
                }
            }
        }
        finally
        {
            lock (_lock)
            {
                _dedicatedInUse--;
            }

            end.TrySetResult();
        }
    }

    // A tick that throws is logged and counted, and its map keeps its place; a map's tick handles its own messages' and timers'
    // faults (spec section 1, "Failure policy").
    private void RunTick(ScheduledMap scheduled, long nowMs)
    {
        try
        {
            scheduled.Map.Tick(nowMs);
        }
        catch (Exception exception)
        {
            MapLog.TickFailed(_logger, scheduled.Map.Name, exception);
            _metrics.TickFailed();
        }
    }

    // Called under the lock.
    private void Reschedule(ScheduledMap scheduled)
    {
        scheduled.DeadlineMs = DeadlineQueue.NextDeadline(scheduled.DeadlineMs, scheduled.Map.TickMs, NowMs, out long skipped);
        if (skipped > 0)
            _metrics.TicksSkipped(skipped);
    }
}
