using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Owners;

/// <summary>
/// Reports an owner that has messages queued but has handled none for the stall time. <see cref="Check"/> is called on one thread (a
/// periodic timer in the host, the test itself in the harness).
/// </summary>
public sealed class OwnerWatchdog
{
    private readonly IOwner[] _owners;
    private readonly TimeProvider _time;
    private readonly TimeSpan _stallAfter;
    private readonly ILogger<OwnerWatchdog> _logger;
    private readonly Dictionary<IOwner, (long Handled, long Since)> _seen = [];

    /// <summary>
    /// Watch <paramref name="owners"/>, reporting one after <paramref name="stallAfter"/> without progress.
    /// </summary>
    public OwnerWatchdog(IEnumerable<IOwner> owners, TimeProvider time, TimeSpan stallAfter, ILogger<OwnerWatchdog> logger)
    {
        _owners = [.. owners];
        _time = time;
        _stallAfter = stallAfter;
        _logger = logger;
    }

    /// <summary>
    /// The names of the owners stalled now, each logged.
    /// </summary>
    public IReadOnlyList<string> Check()
    {
        long now = _time.GetTimestamp();
        var stalled = new List<string>();
        foreach (IOwner owner in _owners)
        {
            long handled = owner.Handled;
            int queued = owner.QueueLength;
            if (queued == 0 || !_seen.TryGetValue(owner, out (long Handled, long Since) seen) || seen.Handled != handled)
            {
                _seen[owner] = (handled, now);
                continue;
            }

            TimeSpan waited = _time.GetElapsedTime(seen.Since, now);
            if (waited >= _stallAfter)
            {
                OwnerLog.Stalled(_logger, owner.Name, waited.TotalSeconds, queued);
                stalled.Add(owner.Name);
            }
        }

        return stalled;
    }
}
