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

        Task stopping = directory.StopAsync();
        _scheduler.TickAll(25);

        await stopping.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
        Assert.Empty(_scheduler.Maps);
        Assert.Null(directory.Host(1));
    }

    // Our own fixture worlds: 1 open, 2 held, 3 instanced, 4 in no table.
    private MapDirectory NewDirectory() => new(
        new FixedWorldCatalog(
        [
            new KeyValuePair<ushort, WorldKind>(1, WorldKind.OpenWorld),
            new KeyValuePair<ushort, WorldKind>(2, WorldKind.Held),
            new KeyValuePair<ushort, WorldKind>(3, WorldKind.Instanced),
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

        public IReadOnlyList<ITickable> Maps
        {
            get
            {
                lock (_maps)
                    return [.. _maps];
            }
        }

        public void Start()
        {
        }

        public void Register(ITickable map, MapPlacement placement)
        {
            lock (_maps)
                _maps.Add(map);
        }

        public Task UnregisterAsync(ITickable map)
        {
            lock (_maps)
                _maps.Remove(map);

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
