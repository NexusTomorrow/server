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
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence;
using NexusTomorrow.Persistence.Account;

namespace NexusTomorrow.Server;

/// <summary>
/// Registers the core: options, persistence, the owners, the gateway and the command layer.
/// </summary>
public static class CoreServices
{
    // An owner with messages queued that has handled none for this long is reported by the watchdog.
    private static readonly TimeSpan s_ownerStallTime = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Register the core against <paramref name="configuration"/>. The clock and the creation catalog are registered only when the
    /// caller has not, so a harness that registers its own first wins.
    /// </summary>
    public static IServiceCollection AddNexusTomorrowCore(this IServiceCollection services, IConfiguration configuration)
    {
        AddOptions<GatewayOptions, GatewayOptionsValidator>(services, configuration, GatewayOptions.Section);
        AddOptions<PersistenceOptions, PersistenceOptionsValidator>(services, configuration, PersistenceOptions.Section);
        AddOptions<RealmOptions, RealmOptionsValidator>(services, configuration, RealmOptions.Section);
        AddOptions<AccountOptions, AccountOptionsValidator>(services, configuration, AccountOptions.Section);
        AddOptions<ContentOptions>(services, configuration, ContentOptions.Section);
        AddOptions<CliOptions>(services, configuration, CliOptions.Section);

        services.TryAddSingleton(TimeProvider.System);
        services.TryAddSingleton<ICreationCatalog, TableCreationCatalog>();

        services.AddSingleton(provider => PersistenceContexts.Create(provider.GetRequiredService<IOptions<PersistenceOptions>>().Value.ConnectionString));
        services.AddSingleton<WriteBehindQueue>();
        services.AddSingleton<IWriteBehind>(provider => provider.GetRequiredService<WriteBehindQueue>());
        services.AddSingleton<IAccountDirectory, AccountDirectory>();

        services.AddSingleton<AccountOwner>();
        services.AddSingleton<RealmOwner>();
        services.AddSingleton<IOwner>(provider => provider.GetRequiredService<AccountOwner>());
        services.AddSingleton<IOwner>(provider => provider.GetRequiredService<RealmOwner>());
        services.AddSingleton(provider => new OwnerWatchdog(
            provider.GetServices<IOwner>(), provider.GetRequiredService<TimeProvider>(), s_ownerStallTime, provider.GetRequiredService<ILogger<OwnerWatchdog>>()));

        services.AddSingleton(MessageCatalog.Default);
        services.AddSingleton(provider => new InboundDecoder(
            provider.GetRequiredService<MessageCatalog>(), OpcodeTable.Pregame, provider.GetRequiredService<IOptions<GatewayOptions>>().Value.DisabledOpcodes));
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

    private static void AddOptions<TOptions>(IServiceCollection services, IConfiguration configuration, string section)
        where TOptions : class
    {
        services.AddOptions<TOptions>().Bind(configuration.GetSection(section)).ValidateDataAnnotations().ValidateOnStart();
    }

    private static void AddOptions<TOptions, TValidator>(IServiceCollection services, IConfiguration configuration, string section)
        where TOptions : class
        where TValidator : class, IValidateOptions<TOptions>
    {
        AddOptions<TOptions>(services, configuration, section);
        services.AddSingleton<IValidateOptions<TOptions>, TValidator>();
    }

    private static CommandDispatcher CreateDispatcher(IServiceProvider provider)
    {
        AccountOwner accounts = provider.GetRequiredService<AccountOwner>();
        TimeProvider time = provider.GetRequiredService<TimeProvider>();
        IWriteBehind persistence = provider.GetRequiredService<IWriteBehind>();
        CommandDefinition[] commands =
        [
            .. AccountCommands.Create(accounts, time),
            .. ServerCommands.Create(accounts, provider.GetServices<IOwner>(), persistence),
        ];
        return new CommandDispatcher(commands, persistence, time, provider.GetRequiredService<ILogger<CommandDispatcher>>());
    }
}
