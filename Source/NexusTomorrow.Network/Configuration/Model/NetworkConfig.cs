using NexusTomorrow.Shared.Configuration;

namespace NexusTomorrow.Network.Configuration.Model;

[ConfigurationBind]
public class NetworkConfig
{
    public required string Host { get; set; }
    public ushort Port { get; set; }
}
