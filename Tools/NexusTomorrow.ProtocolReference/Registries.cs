using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Commands;
using NexusTomorrow.Gateway;
using NexusTomorrow.Maps;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Persistence;
using NexusTomorrow.Server;
using NexusTomorrow.Settings;

namespace NexusTomorrow.ProtocolReference;

/// <summary>
/// The server's command list and settings sections as their pages read them.
/// </summary>
public static class Registries
{
    /// <summary>
    /// The assemblies that hold the server's settings classes, whose documentation files hold the settings' summaries.
    /// </summary>
    public static IReadOnlyList<Assembly> SettingsAssemblies { get; } =
    [
        typeof(GatewayOptions).Assembly,
        typeof(AccountOptions).Assembly,
        typeof(MapOptions).Assembly,
        typeof(PersistenceOptions).Assembly,
        typeof(CliOptions).Assembly,
    ];

    /// <summary>
    /// Every class in <see cref="SettingsAssemblies"/> marked as a settings section, in page order.
    /// </summary>
    public static IReadOnlyList<SettingsSection> SettingsSections() =>
    [
        .. SettingsAssemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsDefined(typeof(SettingsSectionAttribute), inherit: false))
            .Select(SettingsSection.Of)
            .OrderBy(section => section.Order),
    ];

    /// <summary>
    /// Every diagnostic the codec's source generator reports, in id order.
    /// </summary>
    public static IReadOnlyList<CodecDiagnostic> CodecDiagnostics() =>
    [
        .. NexusTomorrow.Network.Codec.Generator.Diagnostics.All
            .Select(descriptor => new CodecDiagnostic(descriptor.Id, descriptor.DefaultSeverity, descriptor.Description.ToString(CultureInfo.InvariantCulture))),
    ];

    /// <summary>
    /// Every command the server registers (<see cref="BuiltinCommands"/>), built over an account owner that never starts: the page reads
    /// the definitions and runs none of them.
    /// </summary>
    public static IReadOnlyList<CommandDefinition> Commands()
    {
        var persistence = new NoPersistence();
        var accounts = new AccountOwner(TimeProvider.System, Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance, persistence);
        return BuiltinCommands.Create(accounts, [accounts], persistence, TimeProvider.System);
    }

    // Nothing runs a command here, so nothing is ever queued.
    private sealed class NoPersistence : IWriteBehind
    {
        public int Backlog => 0;

        public bool IsAvailable => false;

        public void Enqueue(PersistenceWork work) => throw new InvalidOperationException("The reference pages run no command.");
    }
}
