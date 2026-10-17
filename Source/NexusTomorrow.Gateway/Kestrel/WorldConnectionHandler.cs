using System.Net;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NexusTomorrow.Gateway.Connections;
using NexusTomorrow.Gateway.Limits;

namespace NexusTomorrow.Gateway.Kestrel;

/// <summary>
/// Hands each world-port connection to a new <see cref="WorldConnection"/>, within the per-address limit, until the host stops.
/// </summary>
public sealed class WorldConnectionHandler : ConnectionHandler
{
    private const string Listener = "world";

    private readonly IServiceProvider _services;
    private readonly ConnectionGuard _guard;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<WorldConnectionHandler> _logger;

    /// <summary>
    /// Create the handler.
    /// </summary>
    public WorldConnectionHandler(
        IServiceProvider services, ConnectionGuard guard, IHostApplicationLifetime lifetime, ILogger<WorldConnectionHandler> logger)
    {
        _services = services;
        _guard = guard;
        _lifetime = lifetime;
        _logger = logger;
    }

    /// <inheritdoc/>
    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        if (connection.RemoteEndPoint is not IPEndPoint remote)
            return;

        using IDisposable? lease = _guard.TryAcquire(Listener, remote.Address);
        if (lease is null)
        {
            GatewayLog.ConnectionRefused(_logger, Listener, remote);
            return;
        }

        using var stop = CancellationTokenSource.CreateLinkedTokenSource(connection.ConnectionClosed, _lifetime.ApplicationStopping);
        WorldConnection session = _services.GetRequiredService<WorldConnection>();
        await session.RunAsync(connection.Transport, remote, stop.Token);
    }
}
