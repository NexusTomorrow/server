using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Maps.Scheduling;
using NexusTomorrow.Maps.Worlds;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence;

namespace NexusTomorrow.Maps;

/// <summary>
/// One shared map per open world id, made on first entry and run in the pool until the server stops. At construction it warns of
/// every start location in a world it will not host.
/// </summary>
public sealed class MapDirectory : IMapDirectory
{
    private readonly IWorldCatalog _worlds;
    private readonly IMapScheduler _scheduler;
    private readonly IWriteBehind _persistence;
    private readonly MapSettings _settings;
    private readonly Random _random;
    private readonly MapMetrics _metrics;
    private readonly ILoggerFactory _loggers;
    private readonly ILogger<MapDirectory> _logger;
    private readonly Lock _gate = new();
    private readonly Dictionary<ushort, MapOwner> _byWorld = [];
    private MapOwner[] _maps = [];
    private bool _stopped;

    /// <summary>
    /// Create the directory; <paramref name="random"/> is used only under the directory's lock, to seed each map's own generator.
    /// </summary>
    public MapDirectory(
        IWorldCatalog worlds, IMapScheduler scheduler, IWriteBehind persistence, IOptions<MapOptions> options,
        IOptions<RealmOptions> realm, Random random, MapMetrics metrics, ILoggerFactory loggers)
    {
        _worlds = worlds;
        _scheduler = scheduler;
        _persistence = persistence;
        _settings = MapSettings.From(options.Value, realm.Value.Id);
        _random = random;
        _metrics = metrics;
        _loggers = loggers;
        _logger = loggers.CreateLogger<MapDirectory>();

        foreach (StartLocation start in realm.Value.StartLocations)
        {
            WorldKind kind = worlds.KindOf(start.WorldId);
            if (kind != WorldKind.OpenWorld)
                MapLog.StartLocationNotHosted(_logger, start.FactionId, start.WorldId, kind);
        }
    }

    /// <summary>
    /// The maps made so far; a snapshot, readable from any thread.
    /// </summary>
    public IReadOnlyList<MapOwner> Maps => Volatile.Read(ref _maps);

    /// <inheritdoc/>
    public IMapInbox? Host(ushort worldId)
    {
        lock (_gate)
        {
            if (_stopped)
                return null;

            if (_byWorld.TryGetValue(worldId, out MapOwner? existing))
                return existing;

            WorldKind kind = _worlds.KindOf(worldId);
            if (kind != WorldKind.OpenWorld)
            {
                MapLog.WorldNotHosted(_logger, worldId, kind);
                return null;
            }

            var map = new MapOwner(
                worldId, _settings, _persistence, new Random(_random.Next()), _metrics, _loggers.CreateLogger<MapOwner>());
            // The scheduler refuses a registration only while it is stopping; a map it refused would never tick, so it is not kept.
            try
            {
                _scheduler.Register(map, MapPlacement.Pool);
            }
            catch (InvalidOperationException)
            {
                return null;
            }

            _byWorld.Add(worldId, map);
            var grown = new MapOwner[_maps.Length + 1];
            _maps.CopyTo(grown, 0);
            grown[^1] = map;
            Volatile.Write(ref _maps, grown);
            return map;
        }
    }

    /// <summary>
    /// Host no more worlds, stop every map, each handing its characters back with their saves queued, then unregister them. The
    /// scheduler must still be running, since a map stops on its next tick.
    /// </summary>
    public async Task StopAsync()
    {
        MapOwner[] maps;
        lock (_gate)
        {
            _stopped = true;
            maps = _maps;
        }

        var stops = new Task[maps.Length];
        for (int i = 0; i < maps.Length; i++)
            stops[i] = maps[i].StopAsync();

        await Task.WhenAll(stops).ConfigureAwait(false);

        foreach (MapOwner map in maps)
            await _scheduler.UnregisterAsync(map).ConfigureAwait(false);
    }
}
