using System.Net;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NexusTomorrow.Gateway.Limits;
using NexusTomorrow.Gateway.Sts;

namespace NexusTomorrow.Gateway.Kestrel;

/// <summary>
/// Hands each STS-port connection to a new <see cref="StsConnection"/>, within the per-address limit, until the host stops.
/// </summary>
public sealed class StsConnectionHandler : ConnectionHandler
{
    private const string Listener = "sts";

    private readonly IServiceProvider _services;
    private readonly ConnectionGuard _guard;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<StsConnectionHandler> _logger;

    /// <summary>
    /// Create the handler.
    /// </summary>
    public StsConnectionHandler(
        IServiceProvider services, ConnectionGuard guard, IHostApplicationLifetime lifetime, ILogger<StsConnectionHandler> logger)
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
        StsConnection session = _services.GetRequiredService<StsConnection>();
        await session.RunAsync(connection.Transport, remote, stop.Token);
    }
}
