using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.GameTable.Configuration.Model;

namespace NexusTomorrow.GameTable;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Bind <see cref="GameTableConfig"/> from the supplied configuration section, failing validation when <see cref="GameTableConfig.GameTablePath"/> is missing or empty.
    /// </summary>
    public static void AddGameTable(this IServiceCollection sc, IConfigurationSection configuration)
    {
        sc.AddOptions<GameTableConfig>()
            .Bind(configuration)
            .Validate(c => !string.IsNullOrWhiteSpace(c.GameTablePath), $"{nameof(GameTableConfig.GameTablePath)} is required.")
            .ValidateOnStart();
    }
}
