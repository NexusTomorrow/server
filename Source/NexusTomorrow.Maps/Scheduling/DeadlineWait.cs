namespace NexusTomorrow.Maps.Scheduling;

// The platform's best timed wait: a plain timed wait on Windows steps 10 to 16 ms (spec section 1, "Timer"), so Windows uses a
// high-resolution waitable timer; elsewhere WaitAny keeps millisecond precision.
internal static class DeadlineWait
{
    public static IDeadlineWait ForThisPlatform(params WaitHandle[] wakes)
    {
        if (OperatingSystem.IsWindows())
        {
            IDeadlineWait? timer = WindowsDeadlineWait.Create(wakes);
            if (timer is not null)
                return timer;
        }

        return new WaitAnyDeadlineWait(wakes);
    }

    private sealed class WaitAnyDeadlineWait : IDeadlineWait
    {
        private readonly WaitHandle[] _wakes;

        public WaitAnyDeadlineWait(WaitHandle[] wakes)
        {
            _wakes = wakes;
        }

        public void Wait(TimeSpan timeout) => WaitHandle.WaitAny(_wakes, timeout);

        public void Dispose()
        {
        }
    }
}
