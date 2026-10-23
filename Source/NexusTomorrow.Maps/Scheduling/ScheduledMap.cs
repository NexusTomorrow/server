namespace NexusTomorrow.Maps.Scheduling;

// Running, Removed and DeadlineMs change only under the scheduler's lock.
internal sealed class ScheduledMap
{
    public ScheduledMap(ITickable map, long deadlineMs)
    {
        Map = map;
        DeadlineMs = deadlineMs;
    }

    public ITickable Map { get; }

    public long DeadlineMs { get; set; }

    public bool Running { get; set; }

    public bool Removed { get; set; }

    public TaskCompletionSource Unregistered { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
}
