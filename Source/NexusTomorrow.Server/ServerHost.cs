using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NexusTomorrow.Persistence;

namespace NexusTomorrow.Server;

/// <summary>
/// Builds the one process: Kestrel on the three client ports, the owners, the write-behind queue and the console front end.
/// </summary>
public static class ServerHost
{
    /// <summary>
    /// Build the server from <paramref name="args"/> and the usual configuration sources; <paramref name="configure"/> may add
    /// configuration or replace services before the build (the harness replaces the clock).
    /// </summary>
    public static WebApplication Build(string[] args, Action<WebApplicationBuilder>? configure = null)
    {
        WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);
        configure?.Invoke(builder);
        builder.Services.AddNexusTomorrowCore(builder.Configuration);
        builder.WebHost.UseSockets(sockets => sockets.NoDelay = true);
        return builder.Build();
    }

    /// <summary>
    /// Apply every schema's pending migrations; 0 on success, 1 with the error written to standard error.
    /// </summary>
    public static async Task<int> MigrateAsync(IConfiguration configuration, CancellationToken cancellationToken)
    {
        string? connectionString = configuration[$"{PersistenceOptions.Section}:{nameof(PersistenceOptions.ConnectionString)}"];
        if (string.IsNullOrEmpty(connectionString))
        {
            await Console.Error.WriteLineAsync($"{PersistenceOptions.Section}:{nameof(PersistenceOptions.ConnectionString)} is not set.");
            return 1;
        }

        try
        {
            await PersistenceContexts.Create(connectionString).MigrateAsync(cancellationToken);
            return 0;
        }
        catch (Exception exception)
        {
            await Console.Error.WriteLineAsync(exception.Message);
            return 1;
        }
    }
}
