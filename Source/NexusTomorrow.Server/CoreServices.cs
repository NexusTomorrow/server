using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.Commands;
using NexusTomorrow.Gateway;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.Connections;
using NexusTomorrow.Gateway.Kestrel;
using NexusTomorrow.Gateway.Limits;
using NexusTomorrow.Gateway.Sts;
using NexusTomorrow.Maps;
using NexusTomorrow.Maps.Scheduling;
using NexusTomorrow.Maps.Worlds;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;
using NexusTomorrow.Settings;

namespace NexusTomorrow.Server;

/// <summary>
/// Registers the core: options, persistence, the owners, the gateway and the command layer.
/// </summary>
public static class CoreServices
{
    // An owner with messages queued that has handled none for this long is reported by the watchdog.
    private static readonly TimeSpan s_ownerStallTime = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Register the core against <paramref name="configuration"/>. The clock, the creation catalog, the world catalog, the map
    /// scheduler and the random source are registered only when the caller has not, so a harness that registers its own first wins.
    /// </summary>
    public static IServiceCollection AddNexusTomorrowCore(this IServiceCollection services, IConfiguration configuration)
    {
        AddOptions<GatewayOptions>(services, configuration, GatewayOptions.Section);
        AddOptions<PersistenceOptions>(services, configuration, PersistenceOptions.Section);
        AddOptions<RealmOptions>(services, configuration, RealmOptions.Section);
        AddOptions<MapOptions>(services, configuration, MapOptions.Section);
        AddOptions<AccountOptions>(services, configuration, AccountOptions.Section);
        AddOptions<ContentOptions>(services, configuration, ContentOptions.Section);
        AddOptions<CliOptions>(services, configuration, CliOptions.Section);

        services.TryAddSingleton(TimeProvider.System);
        services.TryAddSingleton<ICreationCatalog, TableCreationCatalog>();

        // Random.Shared is safe from any thread; a harness registers its own seeded one first.
        services.TryAddSingleton(Random.Shared);
        services.TryAddSingleton<IWorldCatalog, TableWorldCatalog>();
        services.AddSingleton<MapMetrics>();
        services.TryAddSingleton<IMapScheduler, MapScheduler>();
        services.AddSingleton<MapDirectory>();
        services.AddSingleton<IMapDirectory>(provider => provider.GetRequiredService<MapDirectory>());

        services.AddSingleton(provider => PersistenceContexts.Create(provider.GetRequiredService<IOptions<PersistenceOptions>>().Value.ConnectionString));
        services.AddSingleton<WriteBehindQueue>();
        services.AddSingleton<IWriteBehind>(provider => provider.GetRequiredService<WriteBehindQueue>());
        services.AddSingleton<IAccountDirectory, AccountDirectory>();

        services.AddSingleton<AccountOwner>();
        services.AddSingleton<RealmOwner>();
        services.AddSingleton<IOwner>(provider => provider.GetRequiredService<AccountOwner>());
        services.AddSingleton<IOwner>(provider => provider.GetRequiredService<RealmOwner>());
        services.AddSingleton(provider => new OwnerWatchdog(
            WatchedOwners(provider), provider.GetRequiredService<TimeProvider>(), s_ownerStallTime, provider.GetRequiredService<ILogger<OwnerWatchdog>>()));

        services.AddSingleton(MessageCatalog.Default);
        services.AddSingleton(provider => new InboundDecoder(
            provider.GetRequiredService<MessageCatalog>(), OpcodeTable.All, provider.GetRequiredService<IOptions<GatewayOptions>>().Value.DisabledOpcodes));
        services.AddSingleton<GatewayMetrics>();
        services.AddSingleton<LoginRateLimiter>();
        services.AddSingleton<UnknownAccountCredentials>();
        services.AddSingleton<ConnectionGuard>();
        services.AddTransient<StsConnection>();
        services.AddTransient<LoginConnection>();
        services.AddTransient<WorldConnection>();
        services.AddSingleton<StsConnectionHandler>();
        services.AddSingleton<LoginConnectionHandler>();
        services.AddSingleton<WorldConnectionHandler>();

        services.AddSingleton(provider => CreateDispatcher(provider));

        services.AddHostedService<CoreLifetime>();
        services.AddHostedService<OwnerWatchdogService>();
        services.AddHostedService<ConsoleFrontEnd>();
        return services;
    }

    // Every section is checked against the rules its settings declare (SettingsValidator), once at start.
    private static void AddOptions<TOptions>(IServiceCollection services, IConfiguration configuration, string section)
        where TOptions : class
    {
        services.AddOptions<TOptions>().Bind(configuration.GetSection(section)).ValidateOnStart();
        services.AddSingleton<IValidateOptions<TOptions>, SettingsValidator<TOptions>>();
    }

    // Evaluated afresh on every watchdog check: the registered owners, then every map in MapDirectory.Maps at that moment, so a map
    // made after start-up is watched while it exists.
    private static IEnumerable<IOwner> WatchedOwners(IServiceProvider provider)
    {
        IEnumerable<IOwner> registered = provider.GetServices<IOwner>();
        MapDirectory maps = provider.GetRequiredService<MapDirectory>();
        foreach (IOwner owner in registered)
            yield return owner;
        foreach (MapOwner map in maps.Maps)
            yield return map;
    }

    private static CommandDispatcher CreateDispatcher(IServiceProvider provider)
    {
        AccountOwner accounts = provider.GetRequiredService<AccountOwner>();
        TimeProvider time = provider.GetRequiredService<TimeProvider>();
        IWriteBehind persistence = provider.GetRequiredService<IWriteBehind>();
        IReadOnlyList<CommandDefinition> commands = BuiltinCommands.Create(accounts, provider.GetServices<IOwner>(), persistence, time);
        return new CommandDispatcher(commands, persistence, time, provider.GetRequiredService<ILogger<CommandDispatcher>>());
    }
}
