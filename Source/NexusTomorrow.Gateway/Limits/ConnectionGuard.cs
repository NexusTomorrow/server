using System.Net;
using Microsoft.Extensions.Options;

namespace NexusTomorrow.Gateway.Limits;

/// <summary>
/// Holds each address to <see cref="GatewayOptions.ConnectionsPerAddress"/> connections at once on each listener. A connection past
/// the limit is refused at once, never queued. An IPv4-mapped IPv6 address counts as its IPv4 address.
/// </summary>
public sealed class ConnectionGuard
{
    private readonly int _limit;
    private readonly Lock _gate = new();
    private readonly Dictionary<(string Listener, IPAddress Address), int> _held = [];

    /// <summary>
    /// Create a guard with the limit from <paramref name="options"/>.
    /// </summary>
    public ConnectionGuard(IOptions<GatewayOptions> options)
    {
        _limit = options.Value.ConnectionsPerAddress;
    }

    /// <summary>
    /// Take a slot for <paramref name="address"/> on <paramref name="listener"/>; null when the address holds its limit. Disposing the
    /// lease frees the slot, once however often it is disposed.
    /// </summary>
    public IDisposable? TryAcquire(string listener, IPAddress address)
    {
        (string Listener, IPAddress Address) key = (listener, address.IsIPv4MappedToIPv6 ? address.MapToIPv4() : address);
        lock (_gate)
        {
            _held.TryGetValue(key, out int count);
            if (count >= _limit)
                return null;

            _held[key] = count + 1;
        }

        return new Lease(this, key);
    }

    private void Release((string Listener, IPAddress Address) key)
    {
        lock (_gate)
        {
            if (!_held.TryGetValue(key, out int count))
                return;

            if (count <= 1)
                _held.Remove(key);
            else
                _held[key] = count - 1;
        }
    }

    private sealed class Lease : IDisposable
    {
        private readonly ConnectionGuard _guard;
        private readonly (string Listener, IPAddress Address) _key;
        private int _released;

        public Lease(ConnectionGuard guard, (string Listener, IPAddress Address) key)
        {
            _guard = guard;
            _key = key;
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _released, 1) == 0)
                _guard.Release(_key);
        }
    }
}
