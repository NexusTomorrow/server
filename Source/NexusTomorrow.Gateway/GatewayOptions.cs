using System.ComponentModel.DataAnnotations;

namespace NexusTomorrow.Gateway;

/// <summary>
/// The <c>Gateway</c> configuration section. Everything binds to loopback unless configured otherwise.
/// </summary>
public sealed class GatewayOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Gateway";

    /// <summary>
    /// The STS listener.
    /// </summary>
    public ListenerOptions Sts { get; set; } = new() { Port = 6600 };

    /// <summary>
    /// The login listener.
    /// </summary>
    public ListenerOptions Login { get; set; } = new() { Port = 23115 };

    /// <summary>
    /// The world listener.
    /// </summary>
    public ListenerOptions World { get; set; } = new() { Port = 24000 };

    /// <summary>
    /// The IPv4 address the login server hands the client for the world listener.
    /// </summary>
    public string WorldPublicAddress { get; set; } = "127.0.0.1";

    /// <summary>
    /// The largest frame a client may send, in bytes (decisions.md D9 item 3).
    /// </summary>
    [Range(64, 1 << 20)]
    public int MaxClientFrameBytes { get; set; } = 64 * 1024;

    /// <summary>
    /// Connections one address may hold at once on each listener.
    /// </summary>
    [Range(1, 1000)]
    public int ConnectionsPerAddress { get; set; } = 8;

    /// <summary>
    /// How long a connection may stay open without completing its login.
    /// </summary>
    public TimeSpan PreAuthTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// How many messages one connection may have queued for its client. A client that stops reading fills the queue, and the
    /// connection is disconnected rather than the sender made to wait.
    /// </summary>
    [Range(16, 1 << 20)]
    public int OutboundQueueCapacity { get; set; } = 4096;

    /// <summary>
    /// How long a disconnecting connection may keep trying to deliver what is queued before it is closed regardless, so a client that
    /// has stopped reading cannot hold the connection open.
    /// </summary>
    public TimeSpan DisconnectGrace { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Login attempts (STS login starts and login requests) one address may make per minute.
    /// </summary>
    [Range(1, 1000)]
    public int LoginAttemptsPerMinute { get; set; } = 10;

    /// <summary>
    /// Base64 of at least 32 secret bytes, from which unknown accounts get stable fake credentials. Required; the host refuses to start
    /// without it. Keep it out of the repository.
    /// </summary>
    public required string UnknownAccountSecret { get; set; }

    /// <summary>
    /// Client opcodes switched off: dropped and counted as if they had no table row.
    /// </summary>
    public HashSet<ushort> DisabledOpcodes { get; set; } = [];
}

/// <summary>
/// One listener's address and port.
/// </summary>
public sealed class ListenerOptions
{
    /// <summary>
    /// The address to bind.
    /// </summary>
    public string BindAddress { get; set; } = "127.0.0.1";

    /// <summary>
    /// The TCP port.
    /// </summary>
    [Range(1, 65535)]
    public int Port { get; set; }
}
