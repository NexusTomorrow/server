using System.Globalization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Commands;
using NexusTomorrow.Game.Static;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Server;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Harness;

// The real host, in this process, on free loopback ports with a fake clock and a fixture creation catalog.
public sealed class HarnessServer : IAsyncDisposable
{
    private static readonly TimeSpan s_stopBound = TimeSpan.FromSeconds(30);
    private static readonly CommandActor s_console = new("harness", FrontEnd.Cli, new HashSet<string> { PermissionKeys.All }, null);

    // Our own fixture options, not table rows: one offered, one switched off.
    public static readonly FixedCreationCatalog Catalog = new(
    [
        new CreationOption(1, Race: 1, Sex: 0, Class: 1, FactionId: 166, Enabled: true, CharacterCreationStart.Arkship, Xp: 0, Level: 1, EntitlementRequired: 0, CostAmount: 0, CostumeOnly: false),
        new CreationOption(2, Race: 1, Sex: 1, Class: 1, FactionId: 167, Enabled: false, CharacterCreationStart.Arkship, Xp: 0, Level: 1, EntitlementRequired: 0, CostAmount: 0, CostumeOnly: false),
    ]);

    private readonly WebApplication _app;
    private bool _disposed;

    private HarnessServer(WebApplication app, FakeTimeProvider time, int sts, int login, int world)
    {
        _app = app;
        Time = time;
        StsPort = sts;
        LoginPort = login;
        WorldPort = world;
    }

    public FakeTimeProvider Time { get; }

    public int StsPort { get; }

    public int LoginPort { get; }

    public int WorldPort { get; }

    // Start the host against the database; settings override the harness defaults, key for key.
    public static async Task<HarnessServer> StartAsync(PostgresTestDatabase database, IDictionary<string, string?>? settings = null)
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
        };
        foreach ((string key, string? value) in settings ?? new Dictionary<string, string?>())
            all[key] = value;

        var time = new FakeTimeProvider(new DateTimeOffset(2026, 10, 16, 12, 0, 0, TimeSpan.Zero));
        WebApplication app = ServerHost.Build([], builder =>
        {
            builder.Configuration.AddInMemoryCollection(all);
            builder.Services.AddSingleton<TimeProvider>(time);
            builder.Services.AddSingleton<ICreationCatalog>(Catalog);
        });
        try
        {
            await app.StartAsync();
        }
        catch
        {
            await app.DisposeAsync();
            throw;
        }

        return new HarnessServer(app, time, sts, login, world);
    }

    public T Service<T>()
        where T : notnull => _app.Services.GetRequiredService<T>();

    public Task<CommandReply> RunAsync(string commandLine) =>
        Service<CommandDispatcher>().DispatchAsync(s_console, commandLine, CancellationToken.None);

    public HeadlessClient NewClient() => new(this);

    // Every owner must drain its queue: a stalled owner fails the scenario that stalled it. The host is stopped and disposed even
    // when one does, and a stop that outlasts its bound fails instead of hanging the run.
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;
        try
        {
            foreach (IOwner owner in _app.Services.GetServices<IOwner>())
                await Eventually.TrueAsync(() => owner.QueueLength == 0, TimeSpan.FromSeconds(10));
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
}
