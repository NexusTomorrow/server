using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NexusTomorrow.Harness;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Persistence.Ops;
using NexusTomorrow.Persistence.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Server.Tests;

public sealed class MigrateTests
{
    private const string Password = "hunter2-sentinel";
    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(30);

    [Fact]
    public async Task MigrateAppliesEverySchemaAndExitsZeroAndAgainIsHarmless()
    {
        await using PostgresTestDatabase database = await Databases.CreateAsync(migrate: false);
        var error = new StringWriter();

        Assert.Equal(0, await MigrateAsync(database.ConnectionString, error));
        Assert.Equal(0, await MigrateAsync(database.ConnectionString, error));

        Assert.Equal("", error.ToString());
        await using AccountContext account = database.Contexts.CreateAccount();
        Assert.Empty(await account.Database.GetPendingMigrationsAsync(TestContext.Current.CancellationToken));
        await using RealmContext realm = database.Contexts.CreateRealm();
        Assert.Empty(await realm.Database.GetPendingMigrationsAsync(TestContext.Current.CancellationToken));
        await using OpsContext ops = database.Contexts.CreateOps();
        Assert.Empty(await ops.Database.GetPendingMigrationsAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task MigrateExitsOneWithTheErrorAndNeverTheConnectionString()
    {
        string unreachable = $"Host=127.0.0.1;Port={FreePort.Take().ToString(CultureInfo.InvariantCulture)};Database=nt;Username=nt;Password={Password};Timeout=5";
        var error = new StringWriter();

        Assert.Equal(1, await MigrateAsync(unreachable, error));

        Assert.NotEqual("", error.ToString().Trim());
        Assert.DoesNotContain(Password, error.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain(unreachable, error.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task MigrateExitsOneForAConnectionStringItCannotParseWithoutEchoingIt()
    {
        var error = new StringWriter();

        Assert.Equal(1, await MigrateAsync($"not a connection string {Password}", error));

        Assert.Contains("Persistence:ConnectionString", error.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain(Password, error.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task MigrateExitsOneWhenTheConnectionStringIsNotSet()
    {
        var error = new StringWriter();

        Assert.Equal(1, await MigrateAsync("", error));

        Assert.Contains("Persistence:ConnectionString is not set.", error.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public void ScrubbingRemovesThePasswordAndTheWholeConnectionStringFromAMessage()
    {
        string connectionString = $"Host=db.example;Database=nt;Username=nt;Password={Password}";

        string scrubbed = ServerHost.Scrub($"cannot open {connectionString}; the login {Password} failed", connectionString);

        Assert.DoesNotContain(Password, scrubbed, StringComparison.Ordinal);
        Assert.DoesNotContain("db.example", scrubbed, StringComparison.Ordinal);
    }

    private static async Task<int> MigrateAsync(string connectionString, StringWriter error)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Persistence:ConnectionString"] = connectionString })
            .Build();
        return await ServerHost.MigrateAsync(configuration, error, TestContext.Current.CancellationToken).WaitAsync(s_bound, TestContext.Current.CancellationToken);
    }
}
