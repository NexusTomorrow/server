using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NexusTomorrow.Gateway;
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
    [InlineData(typeof(GatewayOptions), "Gateway:UnknownAccountSecret", "", "Gateway:UnknownAccountSecret is required")]
    [InlineData(typeof(GatewayOptions), "Gateway:UnknownAccountSecret", "AAAAAAAAAAA=", "Gateway:UnknownAccountSecret must be base64 of at least 32 bytes.")]
    [InlineData(typeof(GatewayOptions), "Gateway:UnknownAccountSecret", "this-is-not-base64!", "Gateway:UnknownAccountSecret must be base64 of at least 32 bytes.")]
    [InlineData(typeof(GatewayOptions), "Gateway:WorldPublicAddress", "::1", "Gateway:WorldPublicAddress must be an IPv4 address.")]
    [InlineData(typeof(GatewayOptions), "Gateway:WorldPublicAddress", "not-an-address", "Gateway:WorldPublicAddress must be an IPv4 address.")]
    [InlineData(typeof(GatewayOptions), "Gateway:Login:Port", "6600", "the STS, login and world ports must be three different ports")]
    [InlineData(typeof(GatewayOptions), "Gateway:Sts:Port", "0", "Gateway:Sts: The field Port must be between 1 and 65535.")]
    [InlineData(typeof(GatewayOptions), "Gateway:World:Port", "70000", "Gateway:World: The field Port must be between 1 and 65535.")]
    [InlineData(typeof(GatewayOptions), "Gateway:Login:BindAddress", "localhost", "Gateway:Login:BindAddress must be an IP address.")]
    [InlineData(typeof(GatewayOptions), "Gateway:ConnectionsPerAddress", "0", "ConnectionsPerAddress")]
    [InlineData(typeof(GatewayOptions), "Gateway:OutboundQueueCapacity", "1", "OutboundQueueCapacity")]
    [InlineData(typeof(GatewayOptions), "Gateway:DisconnectGrace", "00:05:00", "DisconnectGrace")]
    [InlineData(typeof(RealmOptions), "Realm:Id", "16384", "between 0 and 16383")]
    [InlineData(typeof(RealmOptions), "Realm:Name", "", "Realm:Name is required.")]
    public void AnInvalidSettingIsRefusedByName(Type options, string key, string value, string expected)
    {
        using WebApplication app = Build(new() { [key] = value });
        OptionsValidationException failure = Assert.Throws<OptionsValidationException>(() => Resolve(app.Services, options));
        Assert.Contains(expected, failure.Message, StringComparison.Ordinal);
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
