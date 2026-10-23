using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NexusTomorrow.Tools;

/// <summary>
/// Registers the oracle's endpoint and verbs.
/// </summary>
public static class OracleServices
{
    /// <summary>
    /// Register the oracle against <paramref name="configuration"/>; its options are validated at start, and the endpoint listens only
    /// when <see cref="OracleOptions.Enabled"/>.
    /// </summary>
    public static IServiceCollection AddOracleTools(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<OracleOptions>().Bind(configuration.GetSection(OracleOptions.Section)).ValidateOnStart();
        services.AddSingleton<IValidateOptions<OracleOptions>, OracleOptionsValidator>();
        services.AddSingleton<OracleVerbs>();
        services.AddHostedService<OracleEndpoint>();
        return services;
    }
}
