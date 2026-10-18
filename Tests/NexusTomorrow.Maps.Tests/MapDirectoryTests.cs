using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Maps.Scheduling;
using NexusTomorrow.Maps.Worlds;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Maps.Tests;

public sealed class MapDirectoryTests : IDisposable
{
    private readonly RecordingScheduler _scheduler = new();
    private readonly MapMetrics _metrics = new();

    public void Dispose() => _metrics.Dispose();

    [Fact]
    public void AnOpenWorldHasOneSharedMap()
    {
        MapDirectory directory = NewDirectory();

        IMapInbox? first = directory.Host(1);
        IMapInbox? second = directory.Host(1);

        Assert.NotNull(first);
        Assert.Same(first, second);
        Assert.Single(_scheduler.Maps);
        MapOwner map = Assert.Single(directory.Maps);
        Assert.Equal(1, map.WorldId);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(0)]
    public void AWorldThatIsNotOpenWorldHasNoMap(ushort worldId)
    {
        MapDirectory directory = NewDirectory();

        Assert.Null(directory.Host(worldId));
        Assert.Empty(_scheduler.Maps);
        Assert.Empty(directory.Maps);
    }

    [Fact]
    public async Task StoppingStopsAndUnregistersEveryMapAndHostsNoMore()
    {
        MapDirectory directory = NewDirectory();
        Assert.NotNull(directory.Host(1));
        Assert.NotNull(directory.Host(5));

        Task stopping = directory.StopAsync();

        Assert.False(stopping.IsCompleted);
        _scheduler.TickAll(25);
        await stopping.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
        Assert.Empty(_scheduler.Maps);
        Assert.Equal([true, true], _scheduler.StoppedWhenUnregistered);
        Assert.Null(directory.Host(1));
    }

    [Fact]
    public async Task AWorldAskedForAfterTheStopBeganHasNoMap()
    {
        MapDirectory directory = NewDirectory();
        Assert.NotNull(directory.Host(1));
        Task stopping = directory.StopAsync();

        Assert.Null(directory.Host(5));

        Assert.Equal(1, _scheduler.RegisterCalls);
        _scheduler.TickAll(25);
        await stopping.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
    }

    [Fact]
    public void ManyCallersAskingForOneWorldMakeOneMap()
    {
        MapDirectory directory = NewDirectory();
        var results = new IMapInbox?[32];

        Parallel.For(0, results.Length, i => results[i] = directory.Host(1));

        Assert.All(results, r => Assert.Same(directory.Maps[0], r));
        Assert.Single(directory.Maps);
        Assert.Equal(1, _scheduler.RegisterCalls);
    }

    [Fact]
    public void AMapTheSchedulerRefusedIsNotKept()
    {
        _scheduler.ThrowOnRegister = true;
        MapDirectory directory = NewDirectory();

        Assert.Null(directory.Host(1));
        Assert.Null(directory.Host(1));

        Assert.Equal(2, _scheduler.RegisterCalls);
        Assert.Empty(directory.Maps);
    }
    // Our own fixture worlds: 1 and 5 open, 2 held, 3 instanced, 4 in no table.
    private MapDirectory NewDirectory() => new(
        new FixedWorldCatalog(
        [
            new KeyValuePair<ushort, WorldKind>(1, WorldKind.OpenWorld),
            new KeyValuePair<ushort, WorldKind>(2, WorldKind.Held),
            new KeyValuePair<ushort, WorldKind>(3, WorldKind.Instanced),
            new KeyValuePair<ushort, WorldKind>(5, WorldKind.OpenWorld),
        ]),
        _scheduler,
        new FakeWriteBehind(),
        Options.Create(new MapOptions()),
        Options.Create(new RealmOptions { Id = 3 }),
        new Random(1),
        _metrics,
        NullLoggerFactory.Instance);

    private sealed class RecordingScheduler : IMapScheduler
    {
        private readonly List<ITickable> _maps = [];
        private readonly List<bool> _stoppedWhenUnregistered = [];
        private int _registerCalls;

        public IReadOnlyList<ITickable> Maps
        {
            get
            {
                lock (_maps)
                    return [.. _maps];
            }
        }

        public bool ThrowOnRegister { get; set; }

        public int RegisterCalls => Volatile.Read(ref _registerCalls);

        public IReadOnlyList<bool> StoppedWhenUnregistered
        {
            get
            {
                lock (_maps)
                    return [.. _stoppedWhenUnregistered];
            }
        }

        public void Start()
        {
        }

        public void Register(ITickable map, MapPlacement placement)
        {
            Interlocked.Increment(ref _registerCalls);
            if (ThrowOnRegister)
                throw new InvalidOperationException("The map scheduler is stopping.");

            lock (_maps)
                _maps.Add(map);
        }

        public Task UnregisterAsync(ITickable map)
        {
            lock (_maps)
            {
                _stoppedWhenUnregistered.Add(((MapOwner)map).StopAsync().IsCompleted);
                _maps.Remove(map);
            }

            return Task.CompletedTask;
        }

        public void TickAll(long nowMs)
        {
            foreach (ITickable map in Maps)
                map.Tick(nowMs);
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
