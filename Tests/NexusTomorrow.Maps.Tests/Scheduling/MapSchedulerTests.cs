using System.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Maps.Scheduling;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Maps.Tests.Scheduling;

// These run the real threads on the real clock, so their bounds are wide: they pin behaviour, not precision.
public sealed class MapSchedulerTests
{
    private static readonly TimeSpan s_fiveSeconds = TimeSpan.FromSeconds(5);

    [Fact]
    public async Task APoolMapTicksAboutOncePerPeriod()
    {
        await using MapScheduler scheduler = Create(poolThreads: 2);
        var map = new CountingMap("pool");
        long start = Stopwatch.GetTimestamp();
        scheduler.Register(map, MapPlacement.Pool);
        scheduler.Start();

        await Task.Delay(1000, TestContext.Current.CancellationToken);
        await scheduler.DisposeAsync();
        double elapsedMs = Stopwatch.GetElapsedTime(start).TotalMilliseconds;

        Assert.InRange(map.Ticks, 15, (long)(elapsedMs / 25) + 2);
    }

    [Fact]
    public async Task AMapNeverTicksOnTwoThreadsAtOnce()
    {
        await using MapScheduler scheduler = Create(poolThreads: 4);
        CountingMap[] maps = [new("a") { Work = 3 }, new("b") { Work = 3 }, new("c") { Work = 3 }];
        foreach (CountingMap map in maps)
            scheduler.Register(map, MapPlacement.Pool);
        scheduler.Start();

        await Task.Delay(500, TestContext.Current.CancellationToken);
        await scheduler.DisposeAsync();

        foreach (CountingMap map in maps)
        {
            Assert.Equal(1, map.MostAtOnce);
            Assert.True(map.Ticks > 0);
        }
    }

    [Fact]
    public async Task OnePoolThreadRunsEveryMap()
    {
        await using MapScheduler scheduler = Create(poolThreads: 1);
        var first = new CountingMap("first");
        var second = new CountingMap("second");
        scheduler.Register(first, MapPlacement.Pool);
        scheduler.Register(second, MapPlacement.Pool);
        scheduler.Start();

        await Eventually.TrueAsync(() => first.Ticks > 3 && second.Ticks > 3);
    }

    [Fact]
    public async Task ADedicatedMapRunsOnAThreadOfItsOwn()
    {
        await using MapScheduler scheduler = Create(poolThreads: 2, dedicatedThreads: 1);
        var map = new CountingMap("capital");
        scheduler.Register(map, MapPlacement.Dedicated);
        scheduler.Start();

        await Eventually.TrueAsync(() => map.Ticks > 0);

        Assert.Equal("map capital", map.LastThreadName);
    }

    [Fact]
    public async Task ADedicatedPlacementPastTheBudgetRunsInThePool()
    {
        await using MapScheduler scheduler = Create(poolThreads: 2, dedicatedThreads: 0);
        var map = new CountingMap("capital");
        scheduler.Register(map, MapPlacement.Dedicated);
        scheduler.Start();

        await Eventually.TrueAsync(() => map.Ticks > 0);

        Assert.StartsWith("map pool", map.LastThreadName);
    }

    [Theory]
    [InlineData(MapPlacement.Pool)]
    [InlineData(MapPlacement.Dedicated)]
    public async Task AnUnregisteredMapTicksNoMore(MapPlacement placement)
    {
        await using MapScheduler scheduler = Create(poolThreads: 2, dedicatedThreads: 1);
        var map = new CountingMap("leaving");
        scheduler.Register(map, placement);
        scheduler.Start();
        await Eventually.TrueAsync(() => map.Ticks > 2);

        await scheduler.UnregisterAsync(map).WaitAsync(s_fiveSeconds, TestContext.Current.CancellationToken);
        long ticks = map.Ticks;
        await Task.Delay(200, TestContext.Current.CancellationToken);

        Assert.Equal(ticks, map.Ticks);
    }

    [Fact]
    public async Task ATickThatThrowsLeavesItsMapScheduled()
    {
        await using MapScheduler scheduler = Create(poolThreads: 2);
        var map = new CountingMap("faulty") { Throws = true };
        scheduler.Register(map, MapPlacement.Pool);
        scheduler.Start();

        await Eventually.TrueAsync(() => map.Ticks > 3);
    }

    [Fact]
    public async Task DisposingStopsEveryThread()
    {
        MapScheduler scheduler = Create(poolThreads: 2, dedicatedThreads: 1);
        var pooled = new CountingMap("pooled");
        var dedicated = new CountingMap("dedicated");
        scheduler.Register(pooled, MapPlacement.Pool);
        scheduler.Register(dedicated, MapPlacement.Dedicated);
        scheduler.Start();
        await Eventually.TrueAsync(() => pooled.Ticks > 0 && dedicated.Ticks > 0);

        await scheduler.DisposeAsync().AsTask().WaitAsync(s_fiveSeconds, TestContext.Current.CancellationToken);
        long pooledTicks = pooled.Ticks;
        long dedicatedTicks = dedicated.Ticks;
        await Task.Delay(200, TestContext.Current.CancellationToken);

        Assert.Equal(pooledTicks, pooled.Ticks);
        Assert.Equal(dedicatedTicks, dedicated.Ticks);
    }

    [Fact]
    public async Task FirstDeadlinesAreSpreadAcrossOneTick()
    {
        await using MapScheduler scheduler = Create(poolThreads: 1);
        long now = scheduler.NowMs;
        var deadlines = new HashSet<long>();

        for (int i = 0; i < 20; i++)
        {
            var map = new CountingMap($"map {i}");
            scheduler.Register(map, MapPlacement.Pool);
            deadlines.Add(scheduler.DeadlineOf(map));
        }

        Assert.All(deadlines, deadline => Assert.InRange(deadline, now, now + 30));
        Assert.True(deadlines.Count > 1);
    }

    private static MapScheduler Create(int poolThreads, int dedicatedThreads = 1)
    {
        var options = new MapOptions { PoolThreads = poolThreads, DedicatedThreads = dedicatedThreads };
        return new MapScheduler(Options.Create(options), TimeProvider.System, new Random(7), new MapMetrics(), NullLogger<MapScheduler>.Instance);
    }

    private sealed class CountingMap : ITickable
    {
        private long _ticks;
        private int _running;
        private int _mostAtOnce;
        private string _lastThreadName = string.Empty;

        public CountingMap(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public int TickMs => 25;

        public int Work { get; init; }

        public bool Throws { get; init; }

        public long Ticks => Interlocked.Read(ref _ticks);

        public int MostAtOnce => Volatile.Read(ref _mostAtOnce);

        public string LastThreadName => Volatile.Read(ref _lastThreadName);

        public void Tick(long nowMs)
        {
            int running = Interlocked.Increment(ref _running);
            int most;
            while (running > (most = Volatile.Read(ref _mostAtOnce)))
                Interlocked.CompareExchange(ref _mostAtOnce, running, most);

            Volatile.Write(ref _lastThreadName, Thread.CurrentThread.Name ?? string.Empty);
            if (Work > 0)
                Thread.Sleep(Work);

            Interlocked.Increment(ref _ticks);
            Interlocked.Decrement(ref _running);
            if (Throws)
                throw new InvalidOperationException("The map failed its tick.");
        }
    }
}
