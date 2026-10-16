using NexusTomorrow.Shared.Configuration;

namespace NexusTomorrow.Network.Configuration.Model;

/// <summary>
/// Where a server's listener binds.
/// </summary>
[ConfigurationBind]
public class NetworkConfig
{
    /// <summary>
    /// The address the listener binds to.
    /// </summary>
    public required string Host { get; set; }

    /// <summary>
    /// The port the listener binds to.
    /// </summary>
    public ushort Port { get; set; }
}
