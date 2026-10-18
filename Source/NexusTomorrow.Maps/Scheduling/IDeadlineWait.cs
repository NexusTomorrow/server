namespace NexusTomorrow.Maps.Scheduling;

// One thread's timed wait: returns when the timeout passes or any wake handle it was made over is signalled.
internal interface IDeadlineWait : IDisposable
{
    void Wait(TimeSpan timeout);
}
