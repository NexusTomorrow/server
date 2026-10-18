namespace NexusTomorrow.Gateway.Catalog;

/// <summary>
/// A token bucket per opcode for one session: each row's allowance refills at its rate per second, up to one second's worth. Used by
/// one connection's read loop only.
/// </summary>
public sealed class RateGate
{
    private readonly TimeProvider _time;
    private readonly Dictionary<ushort, (double Tokens, long Stamp)> _buckets = [];

    /// <summary>
    /// Create a gate that reads time from <paramref name="time"/>.
    /// </summary>
    public RateGate(TimeProvider time)
    {
        _time = time;
    }

    /// <summary>
    /// Take one token from <paramref name="row"/>'s bucket; false when it is empty.
    /// </summary>
    public bool TryPass(OpcodeRow row)
    {
        long now = _time.GetTimestamp();
        (double tokens, long stamp) = _buckets.TryGetValue(row.Opcode, out (double Tokens, long Stamp) bucket) ? bucket : (row.PerSecond, now);
        tokens = Math.Min(row.PerSecond, tokens + (_time.GetElapsedTime(stamp, now).TotalSeconds * row.PerSecond));
        bool pass = tokens >= 1;
        _buckets[row.Opcode] = (pass ? tokens - 1 : tokens, now);
        return pass;
    }
}
