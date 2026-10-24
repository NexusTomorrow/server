using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NexusTomorrow.Gateway;
using NexusTomorrow.Maps;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence;

namespace NexusTomorrow.Server.Tests;

public sealed class OptionsValidationTests
{
    // Values a log must never carry: the connection string's password and the secret, each distinctive enough to search for.
    private const string Password = "hunter2-sentinel";
    private static readonly string s_secret = Convert.ToBase64String(Enumerable.Repeat((byte)0x5A, 40).ToArray());

    [Fact]
    public void ACompleteConfigurationIsAccepted()
    {
        using WebApplication app = Build([]);
        Assert.Equal(s_secret, app.Services.GetRequiredService<IOptions<GatewayOptions>>().Value.UnknownAccountSecret);
        Assert.Equal(4, app.Services.GetRequiredService<IOptions<PersistenceOptions>>().Value.SaveWorkers);
        Assert.Equal("NexusTomorrow", app.Services.GetRequiredService<IOptions<RealmOptions>>().Value.Name);
    }

    [Theory]
    [InlineData(typeof(PersistenceOptions), "Persistence:ConnectionString", "", "Persistence:ConnectionString is required.")]
    [InlineData(typeof(PersistenceOptions), "Persistence:SaveWorkers", "0", "Persistence:SaveWorkers must be at least 1.")]
    [InlineData(typeof(PersistenceOptions), "Persistence:SaveWorkers", "-3", "Persistence:SaveWorkers must be at least 1.")]
    [InlineData(typeof(PersistenceOptions), "Persistence:FirstRetryDelay", "00:00:00", "Persistence:FirstRetryDelay must be positive.")]
    [InlineData(typeof(PersistenceOptions), "Persistence:FirstRetryDelay", "-00:00:01", "Persistence:FirstRetryDelay must be positive.")]
    [InlineData(typeof(PersistenceOptions), "Persistence:MaxRetryDelay", "00:00:00", "Persistence:MaxRetryDelay must be positive.")]
    [InlineData(typeof(PersistenceOptions), "Persistence:FirstRetryDelay", "00:01:00", "FirstRetryDelay must not exceed Persistence:MaxRetryDelay.")]
    [InlineData(typeof(AccountOptions), "Account:TokenLifetime", "00:00:00", "Account:TokenLifetime must be positive.")]
    [InlineData(typeof(AccountOptions), "Account:TokenLifetime", "-00:01:00", "Account:TokenLifetime must be positive.")]
    [InlineData(typeof(GatewayOptions), "Gateway:PreAuthTimeout", "00:00:00", "Gateway:PreAuthTimeout must be positive.")]
    [InlineData(typeof(GatewayOptions), "Gateway:PreAuthTimeout", "-00:00:30", "Gateway:PreAuthTimeout must be positive.")]
    [InlineData(typeof(GatewayOptions), "Gateway:UnknownAccountSecret", "", "Gateway:UnknownAccountSecret is required")]
    [InlineData(typeof(GatewayOptions), "Gateway:UnknownAccountSecret", "AAAAAAAAAAA=", "Gateway:UnknownAccountSecret must be base64 of at least 32 bytes.")]
    [InlineData(typeof(GatewayOptions), "Gateway:UnknownAccountSecret", "this-is-not-base64!", "Gateway:UnknownAccountSecret must be base64 of at least 32 bytes.")]
    [InlineData(typeof(GatewayOptions), "Gateway:WorldPublicAddress", "::1", "Gateway:WorldPublicAddress must be an IPv4 address.")]
    [InlineData(typeof(GatewayOptions), "Gateway:WorldPublicAddress", "not-an-address", "Gateway:WorldPublicAddress must be an IPv4 address.")]
    [InlineData(typeof(GatewayOptions), "Gateway:Login:Port", "6600", "the STS, login and world ports must be three different ports")]
    [InlineData(typeof(GatewayOptions), "Gateway:Sts:Port", "0", "Gateway:Sts:Port must be 1 to 65535.")]
    [InlineData(typeof(GatewayOptions), "Gateway:World:Port", "70000", "Gateway:World:Port must be 1 to 65535.")]
    [InlineData(typeof(GatewayOptions), "Gateway:Login:BindAddress", "localhost", "Gateway:Login:BindAddress must be an IP address.")]
    [InlineData(typeof(GatewayOptions), "Gateway:ConnectionsPerAddress", "0", "Gateway:ConnectionsPerAddress must be 1 to 1000.")]
    [InlineData(typeof(GatewayOptions), "Gateway:OutboundQueueCapacity", "1", "Gateway:OutboundQueueCapacity must be 16 to 1048576.")]
    [InlineData(typeof(GatewayOptions), "Gateway:DisconnectGrace", "00:05:00", "Gateway:DisconnectGrace must be 0 to 60 s.")]
    [InlineData(typeof(GatewayOptions), "Gateway:MaxClientFrameBytes", "63", "Gateway:MaxClientFrameBytes must be 64 to 1048576.")]
    [InlineData(typeof(GatewayOptions), "Gateway:LoginAttemptsPerMinute", "1001", "Gateway:LoginAttemptsPerMinute must be 1 to 1000.")]
    [InlineData(typeof(RealmOptions), "Realm:Id", "16384", "Realm:Id must be 1 to 16383.")]
    [InlineData(typeof(RealmOptions), "Realm:Id", "0", "Realm:Id must be 1 to 16383.")]
    [InlineData(typeof(RealmOptions), "Realm:Name", "", "Realm:Name is required.")]
    [InlineData(typeof(RealmOptions), "Realm:PvpType", "2", "Realm:PvpType must be 0 to 1.")]
    [InlineData(typeof(RealmOptions), "Realm:MaxCharactersPerAccount", "51", "Realm:MaxCharactersPerAccount must be 1 to 50.")]
    [InlineData(typeof(PersistenceOptions), "Persistence:FirstIdBlockTimeout", "00:00:00", "Persistence:FirstIdBlockTimeout must be positive.")]
    [InlineData(typeof(MapOptions), "Maps:TickMs", "30", "Maps:TickMs must be 25, 50, 125 or 250.")]
    [InlineData(typeof(MapOptions), "Maps:ReplicationEveryTicks", "50", "Maps:ReplicationEveryTicks times Maps:TickMs must be 25 to 1000 ms.")]
    [InlineData(typeof(MapOptions), "Maps:ReplicationEveryTicks", "0", "Maps:ReplicationEveryTicks times Maps:TickMs must be 25 to 1000 ms.")]
    [InlineData(typeof(MapOptions), "Maps:PoolThreads", "0", "Maps:PoolThreads must be at least 1.")]
    [InlineData(typeof(MapOptions), "Maps:AdmitRadius", "0", "Maps:AdmitRadius must be at least 1.")]
    [InlineData(typeof(MapOptions), "Maps:AdmitRadius", "200", "AdmitRadius must not exceed Maps:DropRadius.")]
    [InlineData(typeof(MapOptions), "Maps:VisibleCap", "0", "Maps:VisibleCap must be at least 1.")]
    [InlineData(typeof(MapOptions), "Maps:SwapsPerCheck", "0", "Maps:SwapsPerCheck must be at least 1.")]
    [InlineData(typeof(MapOptions), "Maps:DedicatedThreads", "-1", "Maps:DedicatedThreads must not be negative.")]
    [InlineData(typeof(MapOptions), "Maps:SaveInterval", "00:00:00", "Maps:SaveInterval must be at least one second.")]
    [InlineData(typeof(MapOptions), "Maps:NormalDrainFloor", "0", "Maps:NormalDrainFloor must be at least 1.")]
    [InlineData(typeof(MapOptions), "Maps:NormalDrainBudget", "00:00:00", "Maps:NormalDrainBudget must be positive.")]
    public void AnInvalidSettingIsRefusedByName(Type options, string key, string value, string expected)
    {
        using WebApplication app = Build(new() { [key] = value });
        OptionsValidationException failure = Assert.Throws<OptionsValidationException>(() => Resolve(app.Services, options));
        Assert.Contains(expected, failure.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(typeof(GatewayOptions), "Gateway:Sts:Port", "65535")]
    [InlineData(typeof(GatewayOptions), "Gateway:MaxClientFrameBytes", "64")]
    [InlineData(typeof(GatewayOptions), "Gateway:MaxClientFrameBytes", "1048576")]
    [InlineData(typeof(GatewayOptions), "Gateway:DisconnectGrace", "00:00:00")]
    [InlineData(typeof(GatewayOptions), "Gateway:DisconnectGrace", "00:01:00")]
    [InlineData(typeof(GatewayOptions), "Gateway:WorldPublicAddress", "10.0.0.1")]
    [InlineData(typeof(RealmOptions), "Realm:Id", "16383")]
    [InlineData(typeof(RealmOptions), "Realm:MaxCharactersPerAccount", "50")]
    [InlineData(typeof(MapOptions), "Maps:PoolThreads", "1")]
    [InlineData(typeof(MapOptions), "Maps:DedicatedThreads", "0")]
    [InlineData(typeof(MapOptions), "Maps:SaveInterval", "00:00:01")]
    [InlineData(typeof(MapOptions), "Maps:ReplicationEveryTicks", "1")]
    [InlineData(typeof(MapOptions), "Maps:ReplicationEveryTicks", "40")]
    [InlineData(typeof(PersistenceOptions), "Persistence:SaveWorkers", "1")]
    [InlineData(typeof(PersistenceOptions), "Persistence:FirstRetryDelay", "00:00:30")]
    public void ASettingAtTheEdgeOfItsLimitIsAccepted(Type options, string key, string value)
    {
        using WebApplication app = Build(new() { [key] = value });
        Resolve(app.Services, options);
    }

    [Fact]
    public void AccountDefaultsRefuseARepeatedId()
    {
        using WebApplication app = Build(new() { ["AccountDefaults:Entitlements:0:Id"] = "10", ["AccountDefaults:Entitlements:1:Id"] = "10" });
        OptionsValidationException failure = Assert.Throws<OptionsValidationException>(() => Resolve(app.Services, typeof(AccountDefaultsOptions)));
        Assert.Contains(failure.Failures, line => line.StartsWith("AccountDefaults:Entitlements ", StringComparison.Ordinal));
    }

    [Fact]
    public void EveryFailingRuleInASectionIsReported()
    {
        using WebApplication app = Build(new() { ["Maps:PoolThreads"] = "0", ["Maps:TickMs"] = "30" });
        OptionsValidationException failure = Assert.Throws<OptionsValidationException>(() => Resolve(app.Services, typeof(MapOptions)));
        string[] expected = ["Maps:PoolThreads must be at least 1.", "Maps:TickMs must be 25, 50, 125 or 250."];
        Assert.Equal(expected, failure.Failures.Order(StringComparer.Ordinal));
    }

    [Fact]
    public void AMaxRetryDelayThatIsNotPositiveIsReportedWithTheFirstDelayThatExceedsIt()
    {
        // The first delay keeps its default of 100 ms, which exceeds a maximum of zero.
        using WebApplication app = Build(new() { ["Persistence:MaxRetryDelay"] = "00:00:00" });
        OptionsValidationException failure = Assert.Throws<OptionsValidationException>(() => Resolve(app.Services, typeof(PersistenceOptions)));
        string[] expected = ["Persistence:FirstRetryDelay must not exceed Persistence:MaxRetryDelay.", "Persistence:MaxRetryDelay must be positive."];
        Assert.Equal(expected, failure.Failures.Order(StringComparer.Ordinal));
    }

    [Fact]
    public void NoErrorEchoesTheConnectionStringOrAValidSecret()
    {
        using WebApplication app = Build(new() { ["Persistence:SaveWorkers"] = "0", ["Gateway:WorldPublicAddress"] = "::1" });
        OptionsValidationException persistence = Assert.Throws<OptionsValidationException>(() => Resolve(app.Services, typeof(PersistenceOptions)));
        OptionsValidationException gateway = Assert.Throws<OptionsValidationException>(() => Resolve(app.Services, typeof(GatewayOptions)));
        foreach (string message in new[] { persistence.Message, gateway.Message })
        {
            Assert.DoesNotContain(Password, message, StringComparison.Ordinal);
            Assert.DoesNotContain(s_secret, message, StringComparison.Ordinal);
        }
    }

    [Theory]
    [InlineData("c2hvcnQ=")]
    [InlineData("this-is-not-base64!")]
    public void ARefusedSecretIsNotEchoed(string secret)
    {
        using WebApplication app = Build(new() { ["Gateway:UnknownAccountSecret"] = secret });
        OptionsValidationException failure = Assert.Throws<OptionsValidationException>(() => Resolve(app.Services, typeof(GatewayOptions)));
        Assert.Contains("Gateway:UnknownAccountSecret", failure.Message, StringComparison.Ordinal);
        Assert.DoesNotContain(secret, failure.Message, StringComparison.Ordinal);
    }

    // Reading Value runs the validators, as the host's start does.
    private static void Resolve(IServiceProvider services, Type options)
    {
        if (options == typeof(GatewayOptions))
            _ = services.GetRequiredService<IOptions<GatewayOptions>>().Value;
        else if (options == typeof(PersistenceOptions))
            _ = services.GetRequiredService<IOptions<PersistenceOptions>>().Value;
        else if (options == typeof(RealmOptions))
            _ = services.GetRequiredService<IOptions<RealmOptions>>().Value;
        else if (options == typeof(MapOptions))
            _ = services.GetRequiredService<IOptions<MapOptions>>().Value;
        else if (options == typeof(AccountOptions))
            _ = services.GetRequiredService<IOptions<AccountOptions>>().Value;
        else if (options == typeof(AccountDefaultsOptions))
            _ = services.GetRequiredService<IOptions<AccountDefaultsOptions>>().Value;
        else
            throw new ArgumentOutOfRangeException(nameof(options));
    }

    private static WebApplication Build(Dictionary<string, string?> overrides)
    {
        Dictionary<string, string?> settings = new()
        {
            ["Persistence:ConnectionString"] = $"Host=db.example;Database=nt;Username=nt;Password={Password}",
            ["Gateway:UnknownAccountSecret"] = s_secret,
            ["Gateway:Sts:Port"] = "6600",
            ["Gateway:Login:Port"] = "23115",
            ["Gateway:World:Port"] = "24000",
        };
        foreach ((string key, string? value) in overrides)
            settings[key] = value;

        return ServerHost.Build([], builder => builder.Configuration.AddInMemoryCollection(settings));
    }
}
