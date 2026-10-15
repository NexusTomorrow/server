using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.GameTable.Configuration.Model;

namespace NexusTomorrow.GameTable
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Bind and validate <see cref="GameTableConfig"/> from the supplied configuration section.
        /// </summary>
        public static void AddGameTable(this IServiceCollection sc, IConfigurationSection configuration)
        {
            sc.AddOptions<GameTableConfig>()
                .Bind(configuration)
                .ValidateOnStart();
        }
    }
}
