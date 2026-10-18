using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NexusTomorrow.Persistence;
using Npgsql;

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
    public static Task<int> MigrateAsync(IConfiguration configuration, CancellationToken cancellationToken) =>
        MigrateAsync(configuration, Console.Error, cancellationToken);

    internal static async Task<int> MigrateAsync(IConfiguration configuration, TextWriter error, CancellationToken cancellationToken)
    {
        string key = $"{PersistenceOptions.Section}:{nameof(PersistenceOptions.ConnectionString)}";
        string? connectionString = configuration[key];
        if (string.IsNullOrEmpty(connectionString))
        {
            await error.WriteLineAsync($"{key} is not set.");
            return 1;
        }

        try
        {
            _ = new NpgsqlConnectionStringBuilder(connectionString);
        }
        catch (ArgumentException)
        {
            // The parser's message quotes the text it could not read, which may include the password.
            await error.WriteLineAsync($"{key} is not a valid connection string.");
            return 1;
        }

        try
        {
            await PersistenceContexts.Create(connectionString).MigrateAsync(cancellationToken);
            return 0;
        }
        catch (Exception exception)
        {
            await error.WriteLineAsync(Scrub(exception.Message, connectionString));
            return 1;
        }
    }

    // A driver message may quote the connection string or the password; neither reaches the output.
    internal static string Scrub(string message, string connectionString)
    {
        string scrubbed = message.Replace(connectionString, "[connection string]", StringComparison.Ordinal);
        string? password = new NpgsqlConnectionStringBuilder(connectionString).Password;
        return string.IsNullOrEmpty(password) ? scrubbed : scrubbed.Replace(password, "***", StringComparison.Ordinal);
    }
}
