namespace NexusTomorrow.Maps.Scheduling;

// Not thread-safe: the scheduler holds its lock around every call.
internal sealed class DeadlineQueue
{
    private readonly PriorityQueue<ScheduledMap, long> _queue = new();

    public int Count => _queue.Count;

    public void Add(ScheduledMap map) => _queue.Enqueue(map, map.DeadlineMs);

    // The earliest map whose deadline is at or before nowMs, with waitMs 0; null with the time to the front's deadline when
    // the front is not due, or -1 when nothing is queued. Removed maps are dropped as they reach the front.
    public ScheduledMap? TakeDue(long nowMs, out long waitMs)
    {
        while (_queue.TryPeek(out ScheduledMap? front, out long deadlineMs))
        {
            if (front.Removed)
            {
                _queue.Dequeue();
                continue;
            }

            if (deadlineMs > nowMs)
            {
                waitMs = deadlineMs - nowMs;
                return null;
            }

            _queue.Dequeue();
            waitMs = 0;
            return front;
        }

        waitMs = -1;
        return null;
    }

    // The first deadline on the period not yet past, one period on when the clock is behind; the deadlines between are
    // counted in skipped and never replayed.
    public static long NextDeadline(long deadlineMs, int tickMs, long nowMs, out long skipped)
    {
        long periods = nowMs <= deadlineMs ? 1 : Math.Max(1, (nowMs - deadlineMs + tickMs - 1) / tickMs);
        skipped = periods - 1;
        return deadlineMs + (periods * tickMs);
    }
}
