using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Commands;
using NexusTomorrow.Game.Static;
using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Maps;
using NexusTomorrow.Maps.Scheduling;
using NexusTomorrow.Maps.Worlds;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Server;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Harness;

// The real host, in this process, on free loopback ports with a fake clock, a fixture creation catalog, fixture reward properties,
// fixture worlds and maps on virtual time.
public sealed class HarnessServer : IAsyncDisposable
{
    private static readonly TimeSpan s_stopBound = TimeSpan.FromSeconds(30);
    private static readonly CommandActor s_console = new("harness", FrontEnd.Cli, new HashSet<string> { PermissionKeys.All }, null);

    // Our own fixture options, not table rows: one offered, one switched off, and one of faction 167, whose start is in a held world.
    public static readonly FixedCreationCatalog Catalog = new(
    [
        new CreationOption(1, Race: 1, Sex: 0, Class: 1, FactionId: 166, Enabled: true, CharacterCreationStart.Arkship, Xp: 0, Level: 1, EntitlementRequired: 0, CostAmount: 0, CostumeOnly: false),
        new CreationOption(2, Race: 1, Sex: 1, Class: 1, FactionId: 167, Enabled: false, CharacterCreationStart.Arkship, Xp: 0, Level: 1, EntitlementRequired: 0, CostAmount: 0, CostumeOnly: false),
        new CreationOption(3, Race: 1, Sex: 1, Class: 1, FactionId: 167, Enabled: true, CharacterCreationStart.Arkship, Xp: 0, Level: 1, EntitlementRequired: 0, CostAmount: 0, CostumeOnly: false),
    ]);

    // Our own fixture worlds, not table rows: world 1 hosts a map and world 2 is held.
    public static readonly FixedWorldCatalog Worlds = new(
    [
        new KeyValuePair<ushort, WorldKind>(1, WorldKind.OpenWorld),
        new KeyValuePair<ushort, WorldKind>(2, WorldKind.Held),
    ]);

    // Our own fixture reward property, not a table row: the premium tier and one Discrete value, so the world entry ends with them.
    public static readonly FixedRewardProperties Rewards =
        new(1, new RewardPropertyValue((RewardPropertyType)60, 0, RewardModifierValueType.Discrete, 0, 7));

    private readonly WebApplication _app;
    private bool _disposed;

    private HarnessServer(WebApplication app, FakeTimeProvider time, SteppedMapScheduler maps, int sts, int login, int world)
    {
        _app = app;
        Time = time;
        Maps = maps;
        StsPort = sts;
        LoginPort = login;
        WorldPort = world;
    }

    // Faction 166's start in world 1, zone 3 (our own numbers).
    public static Vector3 Start { get; } = new(100, 10, -200);

    public FakeTimeProvider Time { get; }

    // The scheduler every map registers with; the host starts it running, and a scenario pauses it to step the maps by hand.
    public SteppedMapScheduler Maps { get; }

    public int StsPort { get; }

    public int LoginPort { get; }

    public int WorldPort { get; }

    // Start the host against the database; settings override the harness defaults, key for key.
    public static async Task<HarnessServer> StartAsync(PostgresTestDatabase database, IDictionary<string, string?>? settings = null, int seed = 1)
    {
        int sts = FreePort.Take();
        int login = FreePort.Take();
        int world = FreePort.Take();
        var all = new Dictionary<string, string?>
        {
            ["Persistence:ConnectionString"] = database.ConnectionString,
            ["Gateway:UnknownAccountSecret"] = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            ["Gateway:Sts:Port"] = sts.ToString(CultureInfo.InvariantCulture),
            ["Gateway:Login:Port"] = login.ToString(CultureInfo.InvariantCulture),
            ["Gateway:World:Port"] = world.ToString(CultureInfo.InvariantCulture),
            ["Cli:Enabled"] = "false",
            ["Maps:SaveInterval"] = "01:00:00",
            ["Realm:StartLocations:0:FactionId"] = "166",
            ["Realm:StartLocations:0:WorldId"] = "1",
            ["Realm:StartLocations:0:ZoneId"] = "3",
            ["Realm:StartLocations:0:X"] = Start.X.ToString(CultureInfo.InvariantCulture),
            ["Realm:StartLocations:0:Y"] = Start.Y.ToString(CultureInfo.InvariantCulture),
            ["Realm:StartLocations:0:Z"] = Start.Z.ToString(CultureInfo.InvariantCulture),
            ["Realm:StartLocations:1:FactionId"] = "167",
            ["Realm:StartLocations:1:WorldId"] = "2",
        };
        foreach ((string key, string? value) in settings ?? new Dictionary<string, string?>())
            all[key] = value;

        var time = new FakeTimeProvider(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));
        var maps = new SteppedMapScheduler();
        WebApplication app = ServerHost.Build([], builder =>
        {
            builder.Configuration.AddInMemoryCollection(all);
            builder.Services.AddSingleton<TimeProvider>(time);
            builder.Services.AddSingleton<ICreationCatalog>(Catalog);
            builder.Services.AddSingleton<IRewardProperties>(Rewards);
            builder.Services.AddSingleton<IWorldCatalog>(Worlds);
            builder.Services.AddSingleton(new Random(seed));
            builder.Services.AddSingleton(maps);
            builder.Services.AddSingleton<IMapScheduler>(maps);
        });
        try
        {
            await app.StartAsync();
        }
        catch
        {
            await app.DisposeAsync();
            await maps.DisposeAsync();
            throw;
        }

        return new HarnessServer(app, time, maps, sts, login, world);
    }

    public T Service<T>()
        where T : notnull => _app.Services.GetRequiredService<T>();

    public Task<CommandReply> RunAsync(string commandLine) =>
        Service<CommandDispatcher>().DispatchAsync(s_console, commandLine, CancellationToken.None);

    public HeadlessClient NewClient() => new(this);

    // The directory's one map of the world.
    public MapOwner Map(ushort worldId) => Service<MapDirectory>().Maps.Single(map => map.WorldId == worldId);

    // The maps run again first, so each drains and the host's stop can hand its characters back. Every owner and every map must
    // drain its queue: a stalled one fails the scenario that stalled it. The host is stopped and disposed even when one does, and a
    // stop that outlasts its bound fails instead of hanging the run.
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;
        try
        {
            try
            {
                Maps.Resume();
                foreach (IOwner owner in _app.Services.GetServices<IOwner>())
                    await Eventually.TrueAsync(() => owner.QueueLength == 0, TimeSpan.FromSeconds(10));
                foreach (MapOwner map in Service<MapDirectory>().Maps)
                    await Eventually.TrueAsync(() => map.QueueLength == 0, TimeSpan.FromSeconds(10));
            }
            finally
            {
                try
                {
                    await _app.StopAsync().WaitAsync(s_stopBound);
                }
                finally
                {
                    await _app.DisposeAsync().AsTask().WaitAsync(s_stopBound);
                }
            }
        }
        catch
        {
            // The host's stop and dispose have run, each up to its 30 s bound. A map whose tick keeps throwing can hold its queue past
            // the drain's bound (10 s for each owner's and each map's queue in turn, not for the drain as a whole) or the stop past its
            // own; a fault a tick held surfaces here, with its own stack, in place of whatever threw. With no fault held, what threw
            // surfaces unchanged.
            Maps.ThrowIfFaulted();
            throw;
        }

        // Nothing threw, so the host stopped and disposed within its bounds; a fault a tick held still fails the scenario that broke it.
        Maps.ThrowIfFaulted();
    }
}
