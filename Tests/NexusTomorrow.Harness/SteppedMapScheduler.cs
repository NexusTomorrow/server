using System.Runtime.ExceptionServices;
using NexusTomorrow.Maps.Scheduling;

namespace NexusTomorrow.Harness;

// Maps tick on virtual time, one step at a time, on a background loop from Start until PauseAsync or on the caller's thread through
// TickAsync; once paused, a map ticks only when the caller steps it, so a scenario is deterministic. A step adds StepMs to NowMs and
// ticks, in registration order, every map whose deadline has come. Steps never overlap: each runs under one semaphore, and
// UnregisterAsync takes the same one, so a map is never removed mid-step. As in the real scheduler, a tick that throws keeps its
// map in place and the step goes on to the next map; the first such exception is held (later ones in the same window are dropped)
// and thrown, with its own stack, once by the next TickAsync, PauseAsync or ThrowIfFaulted, which then clears it. The background
// loop never stops for a fault, so the host's stop can still tick its maps.
public sealed class SteppedMapScheduler : IMapScheduler
{
    public const int StepMs = 25;

    private readonly Lock _gate = new();
    private readonly SemaphoreSlim _step = new(1, 1);
    private readonly List<Entry> _entries = [];
    private long _nowMs;
    private CancellationTokenSource? _loopStop;
    private Task _loop = Task.CompletedTask;
    private ExceptionDispatchInfo? _fault;
    private bool _disposed;

    // The virtual clock the maps tick on, in milliseconds; it moves only inside a step.
    public long NowMs => Volatile.Read(ref _nowMs);

    public void Start() => Resume();

    // Run the background loop (a step, then a 1 ms real delay) unless it runs or the scheduler is disposed.
    public void Resume()
    {
        lock (_gate)
        {
            if (_disposed || _loopStop is not null)
                return;

            var stop = new CancellationTokenSource();
            _loopStop = stop;
            CancellationToken token = stop.Token;
            _loop = Task.Run(() => RunAsync(token));
        }
    }

    // Stop the background loop and wait for its current step to end, then throw a fault a tick left.
    public async Task PauseAsync()
    {
        await StopLoopAsync();
        ThrowIfFaulted();
    }

    // Throw, once, the first exception a tick has thrown since the last report; nothing when there is none.
    public void ThrowIfFaulted()
    {
        ExceptionDispatchInfo? fault;
        lock (_gate)
        {
            fault = _fault;
            _fault = null;
        }

        fault?.Throw();
    }

    private async Task StopLoopAsync()
    {
        CancellationTokenSource? stop;
        Task loop;
        lock (_gate)
        {
            stop = _loopStop;
            loop = _loop;
            _loopStop = null;
            _loop = Task.CompletedTask;
        }

        if (stop is null)
            return;

        try
        {
            await stop.CancelAsync();
            await loop;
        }
        finally
        {
            stop.Dispose();
        }
    }

    // Take the steps one at a time on the caller's thread.
    public async Task TickAsync(int steps = 1)
    {
        for (int i = 0; i < steps; i++)
        {
            await _step.WaitAsync();
            try
            {
                Step();
            }
            finally
            {
                _step.Release();
            }

            ThrowIfFaulted();
        }
    }

    // Check the condition before each step, take at most maxSteps, and return the condition's value at the end.
    public async Task<bool> TickUntilAsync(Func<bool> condition, int maxSteps)
    {
        for (int i = 0; i < maxSteps && !condition(); i++)
            await TickAsync();

        return condition();
    }

    // The map ticks on the next step; placement is ignored.
    public void Register(ITickable map, MapPlacement placement)
    {
        lock (_gate)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_entries.Exists(entry => entry.Map == map))
                throw new InvalidOperationException($"Map {map.Name} is already registered.");

            _entries.Add(new Entry(map, NowMs));
        }
    }

    public async Task UnregisterAsync(ITickable map)
    {
        // Disposal waits for the loop's last step before it disposes the semaphore, so a wait that finds it gone has nothing to
        // exclude.
        bool held = true;
        try
        {
            await _step.WaitAsync();
        }
        catch (ObjectDisposedException)
        {
            held = false;
        }

        try
        {
            lock (_gate)
                _entries.RemoveAll(entry => entry.Map == map);
        }
        finally
        {
            if (held)
                _step.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        lock (_gate)
        {
            if (_disposed)
                return;

            _disposed = true;
        }

        await StopLoopAsync();
        _step.Dispose();
    }

    private async Task RunAsync(CancellationToken stop)
    {
        try
        {
            while (true)
            {
                await _step.WaitAsync(stop);
                try
                {
                    Step();
                }
                finally
                {
                    _step.Release();
                }

                await Task.Delay(1, stop);
            }
        }
        catch (OperationCanceledException) when (stop.IsCancellationRequested)
        {
        }
    }

    // Called with the step semaphore held, so the clock and the deadlines move on one thread at a time.
    private void Step()
    {
        long now = NowMs + StepMs;
        Volatile.Write(ref _nowMs, now);
        Entry[] snapshot;
        lock (_gate)
            snapshot = [.. _entries];

        foreach (Entry entry in snapshot)
        {
            if (entry.DeadlineMs > now)
                continue;

            entry.DeadlineMs = now + entry.Map.TickMs;
            try
            {
                entry.Map.Tick(now);
            }
            catch (Exception exception)
            {
                lock (_gate)
                    _fault ??= ExceptionDispatchInfo.Capture(exception);
            }
        }
    }

    private sealed class Entry
    {
        public Entry(ITickable map, long deadlineMs)
        {
            Map = map;
            DeadlineMs = deadlineMs;
        }

        public ITickable Map { get; }

        public long DeadlineMs { get; set; }
    }
}
