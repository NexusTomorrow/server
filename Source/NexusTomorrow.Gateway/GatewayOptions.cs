using NexusTomorrow.Settings;

namespace NexusTomorrow.Gateway;

/// <summary>
/// The <c>Gateway</c> configuration section. Everything binds to loopback unless configured otherwise.
/// </summary>
[SettingsSection(Section, 1, "Gateway", "Gateway#knobs")]
[DistinctSettings("ports", "the STS, login and world ports", "Sts:Port", "Login:Port", "World:Port")]
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
    [IPv4Address]
    public string WorldPublicAddress { get; set; } = "127.0.0.1";

    /// <summary>
    /// The largest frame a client may send, in bytes.
    /// </summary>
    /// <remarks>
    /// decisions.md D9 item 3.
    /// </remarks>
    [Unit("bytes")]
    [Between(64, 1 << 20)]
    public int MaxClientFrameBytes { get; set; } = 64 * 1024;

    /// <summary>
    /// Connections one address may hold at once on each listener.
    /// </summary>
    [Between(1, 1000)]
    public int ConnectionsPerAddress { get; set; } = 8;

    /// <summary>
    /// How long a connection may stay open without completing its login.
    /// </summary>
    /// <remarks>
    /// Positive, because <c>StsConnection</c> arms each connection's timer with it: zero ends every connection at once, and a negative
    /// value throws, or at -1 ms switches the limit off.
    /// </remarks>
    [Positive]
    public TimeSpan PreAuthTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// How many messages one connection may have queued for its client; a client that stops reading fills the queue, and the
    /// connection is disconnected rather than the sender made to wait.
    /// </summary>
    [Between(16, 1 << 20)]
    public int OutboundQueueCapacity { get; set; } = 4096;

    /// <summary>
    /// How long a disconnecting connection may keep trying to deliver what is queued before it is closed regardless, so a client that
    /// has stopped reading cannot hold the connection open; zero closes at once.
    /// </summary>
    [BetweenDurations("00:00:00", "00:01:00")]
    public TimeSpan DisconnectGrace { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Login attempts (STS login starts and login requests) one address may make per minute.
    /// </summary>
    [Between(1, 1000)]
    public int LoginAttemptsPerMinute { get; set; } = 10;

    /// <summary>
    /// The key from which unknown accounts get stable fake credentials; keep it out of the repository.
    /// </summary>
    [Secret]
    [Base64Bytes(32)]
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
    [IPAddress]
    public string BindAddress { get; set; } = "127.0.0.1";

    /// <summary>
    /// The TCP port.
    /// </summary>
    [Between(1, 65535)]
    public int Port { get; set; }
}
