using System.Net;
using Microsoft.Extensions.Options;

namespace NexusTomorrow.Gateway.Limits;

/// <summary>
/// Caps login attempts per address: each address may make <see cref="GatewayOptions.LoginAttemptsPerMinute"/> attempts in any minute.
/// An address with no attempt in the last minute holds no memory, and a sweep once a minute drops the addresses that never came back.
/// </summary>
public sealed class LoginRateLimiter
{
    private static readonly TimeSpan s_window = TimeSpan.FromMinutes(1);

    private readonly TimeProvider _time;
    private readonly int _attemptsPerMinute;
    private readonly Lock _gate = new();
    private readonly Dictionary<IPAddress, Queue<long>> _attempts = [];
    private long _lastSweep;

    /// <summary>
    /// Create a limiter that reads time from <paramref name="time"/>.
    /// </summary>
    public LoginRateLimiter(TimeProvider time, IOptions<GatewayOptions> options)
    {
        _time = time;
        _attemptsPerMinute = options.Value.LoginAttemptsPerMinute;
        _lastSweep = time.GetTimestamp();
    }

    /// <summary>
    /// The addresses with attempts in memory.
    /// </summary>
    internal int TrackedAddresses
    {
        get
        {
            lock (_gate)
                return _attempts.Count;
        }
    }

    /// <summary>
    /// Record an attempt from <paramref name="address"/>; false, and nothing recorded, when it has used its attempts for the minute.
    /// </summary>
    public bool TryAttempt(IPAddress address)
    {
        IPAddress key = address.IsIPv4MappedToIPv6 ? address.MapToIPv4() : address;
        lock (_gate)
        {
            long now = _time.GetTimestamp();
            if (_time.GetElapsedTime(_lastSweep, now) >= s_window)
            {
                Sweep(now);
                _lastSweep = now;
            }

            if (!_attempts.TryGetValue(key, out Queue<long>? stamps))
                stamps = _attempts[key] = new Queue<long>();

            Expire(stamps, now);
            if (stamps.Count >= _attemptsPerMinute)
                return false;

            stamps.Enqueue(now);
            return true;
        }
    }

    private void Sweep(long now)
    {
        foreach ((IPAddress address, Queue<long> stamps) in _attempts)
        {
            Expire(stamps, now);
            if (stamps.Count == 0)
                _attempts.Remove(address);
        }
    }

    private void Expire(Queue<long> stamps, long now)
    {
        while (stamps.TryPeek(out long oldest) && _time.GetElapsedTime(oldest, now) >= s_window)
            stamps.Dequeue();
    }
}
