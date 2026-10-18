using System.Diagnostics.Metrics;

namespace NexusTomorrow.Gateway;

/// <summary>
/// The gateway's counters, published through <see cref="System.Diagnostics.Metrics"/>.
/// </summary>
public sealed class GatewayMetrics : IDisposable
{
    private readonly Meter _meter = new("NexusTomorrow.Gateway");
    private readonly Counter<long> _dropped;

    /// <summary>
    /// Create the counters.
    /// </summary>
    public GatewayMetrics()
    {
        _dropped = _meter.CreateCounter<long>("nexustomorrow.gateway.messages_dropped", description: "Client messages dropped without being read");
    }

    /// <summary>
    /// Count one dropped client message.
    /// </summary>
    public void Dropped(ushort opcode, string reason) =>
        _dropped.Add(1, new KeyValuePair<string, object?>("opcode", opcode), new KeyValuePair<string, object?>("reason", reason));

    /// <inheritdoc/>
    public void Dispose() => _meter.Dispose();
}
