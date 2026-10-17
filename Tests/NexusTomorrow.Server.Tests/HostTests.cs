using System.Globalization;
using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence;
using NexusTomorrow.Testing;
using Npgsql;

namespace NexusTomorrow.Server.Tests;

public sealed class HostTests
{
    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(30);

    [Fact]
    public async Task TheHostRefusesToStartWithoutTheUnknownAccountSecret()
    {
        await using WebApplication app = ServerHost.Build([], builder =>
            builder.Configuration.AddInMemoryCollection(Settings("Host=unused", secret: null)));
        Exception failure = await Assert.ThrowsAnyAsync<Exception>(() => app.StartAsync(TestContext.Current.CancellationToken));
        Assert.Contains("UnknownAccountSecret", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task TheHostRefusesToStartWithPendingMigrations()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync(migrate: false);
        await using WebApplication app = ServerHost.Build([], builder =>
            builder.Configuration.AddInMemoryCollection(Settings(database.ConnectionString, Convert.ToBase64String(new byte[32]))));
        InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() => app.StartAsync(TestContext.Current.CancellationToken));
        Assert.Contains("migrate", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task TheHostListensGreetsAndStopsCleanly()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        Dictionary<string, string?> settings = Settings(database.ConnectionString, Convert.ToBase64String(new byte[32]));
        int loginPort = int.Parse(settings["Gateway:Login:Port"]!, CultureInfo.InvariantCulture);
        await using WebApplication app = ServerHost.Build([], builder => builder.Configuration.AddInMemoryCollection(settings));
        await app.StartAsync(TestContext.Current.CancellationToken);

        using var client = new TcpClient();
        await client.ConnectAsync("127.0.0.1", loginPort, TestContext.Current.CancellationToken);
        byte[] header = new byte[6];
        await client.GetStream().ReadExactlyAsync(header, TestContext.Current.CancellationToken);
        Assert.Equal(0x0003, BitConverter.ToUInt16(header, 4)); // the bare greeting

        await app.StopAsync(TestContext.Current.CancellationToken).WaitAsync(TimeSpan.FromSeconds(30), TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task TheRealmOwnerHasHandledItsFirstIdBlockOnceTheHostHasStarted()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        Dictionary<string, string?> settings = Settings(database.ConnectionString, Convert.ToBase64String(new byte[32]));
        await using WebApplication app = ServerHost.Build([], builder => builder.Configuration.AddInMemoryCollection(settings));
        await app.StartAsync(TestContext.Current.CancellationToken);

        // Nothing else posts to the realm owner before a client connects, so one handled message is the id block.
        RealmOwner realm = app.Services.GetRequiredService<RealmOwner>();
        await Eventually.TrueAsync(() => realm.Handled == 1);
        await app.StopAsync(TestContext.Current.CancellationToken).WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task AnAddressPastItsConnectionLimitIsRefusedAndAClosedConnectionFreesItsSlot()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        Dictionary<string, string?> settings = Settings(database.ConnectionString, Convert.ToBase64String(new byte[32]));
        settings["Gateway:ConnectionsPerAddress"] = "2";
        int loginPort = int.Parse(settings["Gateway:Login:Port"]!, CultureInfo.InvariantCulture);
        await using WebApplication app = ServerHost.Build([], builder => builder.Configuration.AddInMemoryCollection(settings));
        await app.StartAsync(TestContext.Current.CancellationToken);
        using var bound = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        bound.CancelAfter(s_bound);

        using var first = new TcpClient();
        using var second = new TcpClient();
        Assert.True(await GreetedAsync(first, loginPort, bound.Token));
        Assert.True(await GreetedAsync(second, loginPort, bound.Token));

        using var refused = new TcpClient();
        Assert.False(await GreetedAsync(refused, loginPort, bound.Token));

        first.Dispose();
        bool admitted = false;
        while (!admitted)
        {
            using var retry = new TcpClient();
            admitted = await GreetedAsync(retry, loginPort, bound.Token);
            if (!admitted)
                await Task.Delay(20, bound.Token);
        }

        await app.StopAsync(TestContext.Current.CancellationToken).WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task StoppingWaitsForEveryQueuedSaveAndReportsTheBacklogUntilItCommits()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        Dictionary<string, string?> settings = Settings(database.ConnectionString, Convert.ToBase64String(new byte[32]));
        var time = new FakeTimeProvider();
        var logs = new CapturingLoggerProvider();
        await using WebApplication app = ServerHost.Build([], builder =>
        {
            builder.Configuration.AddInMemoryCollection(settings);
            builder.Services.AddSingleton<TimeProvider>(time);
            builder.Logging.AddProvider(logs);
        });
        await app.StartAsync(TestContext.Current.CancellationToken);
        WriteBehindQueue queue = app.Services.GetRequiredService<WriteBehindQueue>();
        GatedWork gated = new();
        queue.Enqueue(gated);
        await gated.Started.WaitAsync(s_bound, TestContext.Current.CancellationToken);
        await Eventually.TrueAsync(() => queue.Backlog == 1);

        // The token is cancelled once the wait is under way: the saves are waited for regardless.
        using var shutdown = new CancellationTokenSource();
        Task stopping = app.StopAsync(shutdown.Token);
        const string waiting = "Still waiting for 1 queued saves to commit; the database is reachable: True";
        using var bound = new CancellationTokenSource(s_bound);
        while (!logs.Lines.Contains(waiting))
        {
            time.Advance(CoreLifetime.WaitLogInterval);
            await Task.Delay(10, bound.Token);
        }

        await shutdown.CancelAsync();
        Assert.False(stopping.IsCompleted);

        gated.Release();
        await stopping.WaitAsync(s_bound, TestContext.Current.CancellationToken);
        Assert.Equal(0, queue.Backlog);
    }

    [Fact]
    public async Task AStartThatCannotTakeTheFirstIdBlockFailsWithItsCauseAndTheHostDisposesWithoutHanging()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync();
        await using (var connection = new NpgsqlConnection(database.ConnectionString))
        {
            // With the sequence gone the block-take work fails on every attempt, so it retries and never commits.
            await connection.OpenAsync(TestContext.Current.CancellationToken);
            await using var drop = new NpgsqlCommand("DROP SEQUENCE realm.character_id_seq CASCADE", connection);
            await drop.ExecuteNonQueryAsync(TestContext.Current.CancellationToken);
        }

        var time = new FakeTimeProvider();
        Dictionary<string, string?> settings = Settings(database.ConnectionString, Convert.ToBase64String(new byte[32]));
        WebApplication app = ServerHost.Build([], builder =>
        {
            builder.Configuration.AddInMemoryCollection(settings);
            builder.Services.AddSingleton<TimeProvider>(time);
        });
        Task starting = app.StartAsync(TestContext.Current.CancellationToken);
        using var bound = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        bound.CancelAfter(s_bound);

        // A step before the start waits on the clock is lost, so the clock moves a whole timeout at a time until the start gives up.
        while (!starting.IsCompleted)
        {
            time.Advance(TimeSpan.FromSeconds(30));
            await Task.Delay(10, bound.Token);
        }

        InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() => starting);
        Assert.Contains("first character id block", failure.Message, StringComparison.Ordinal);
        await app.DisposeAsync().AsTask().WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }

    [Theory]
    [InlineData("Account:TokenLifetime", "not-a-time")]
    [InlineData("Cli:Enabled", "maybe")]
    public async Task ABadValueInAnyOptionsSectionFailsTheStartNamingItBeforeTheDatabaseIsTouched(string key, string value)
    {
        Dictionary<string, string?> settings = Settings("Host=unused", Convert.ToBase64String(new byte[32]));
        settings[key] = value;
        await using WebApplication app = ServerHost.Build([], builder => builder.Configuration.AddInMemoryCollection(settings));

        // The database named by the connection string does not exist: a start that got as far as the migration check would fail
        // with a connection error instead of naming the key.
        Exception failure = await Assert.ThrowsAnyAsync<Exception>(() => app.StartAsync(TestContext.Current.CancellationToken));

        Assert.Contains(key, failure.Message, StringComparison.Ordinal);
    }

    // Connects and reports whether the server greeted the connection; a connection the server refuses is closed with no bytes sent.
    private static async Task<bool> GreetedAsync(TcpClient client, int port, CancellationToken cancellationToken)
    {
        await client.ConnectAsync("127.0.0.1", port, cancellationToken);
        byte[] header = new byte[6];
        try
        {
            await client.GetStream().ReadExactlyAsync(header, cancellationToken);
            return true;
        }
        catch (EndOfStreamException)
        {
            return false;
        }
        catch (IOException)
        {
            return false;
        }
    }

    private static Dictionary<string, string?> Settings(string connectionString, string? secret) => new()
    {
        ["Persistence:ConnectionString"] = connectionString,
        ["Gateway:UnknownAccountSecret"] = secret,
        ["Gateway:Sts:Port"] = FreePort.Take().ToString(CultureInfo.InvariantCulture),
        ["Gateway:Login:Port"] = FreePort.Take().ToString(CultureInfo.InvariantCulture),
        ["Gateway:World:Port"] = FreePort.Take().ToString(CultureInfo.InvariantCulture),
        ["Cli:Enabled"] = "false",
    };

    // Work that holds its queue worker until the test releases it, and commits nothing.
    private sealed class GatedWork : PersistenceWork
    {
        private readonly TaskCompletionSource _started = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource _release = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public GatedWork()
            : base(PersistKey.Named("gated-save"))
        {
        }

        public Task Started => _started.Task;

        public void Release() => _release.TrySetResult();

        public override async Task ApplyAsync(PersistenceContexts contexts, CancellationToken cancellationToken)
        {
            _started.TrySetResult();
            await _release.Task.WaitAsync(cancellationToken);
        }

        public override void Complete(WorkResult result)
        {
        }
    }
}
