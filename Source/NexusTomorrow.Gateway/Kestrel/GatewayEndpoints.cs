using System.Net;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace NexusTomorrow.Gateway.Kestrel;

/// <summary>
/// The three client listeners.
/// </summary>
public static class GatewayEndpoints
{
    /// <summary>
    /// Bind the STS, login and world listeners to the addresses and ports in <paramref name="options"/>, each served by its connection
    /// handler.
    /// </summary>
    public static void Listen(KestrelServerOptions kestrel, GatewayOptions options)
    {
        kestrel.Listen(IPAddress.Parse(options.Sts.BindAddress), options.Sts.Port, listen => listen.UseConnectionHandler<StsConnectionHandler>());
        kestrel.Listen(IPAddress.Parse(options.Login.BindAddress), options.Login.Port, listen => listen.UseConnectionHandler<LoginConnectionHandler>());
        kestrel.Listen(IPAddress.Parse(options.World.BindAddress), options.World.Port, listen => listen.UseConnectionHandler<WorldConnectionHandler>());
    }
}
