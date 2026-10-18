using System.Diagnostics;
using NexusTomorrow.Maps.Scheduling;

namespace NexusTomorrow.Maps.Tests.Scheduling;

public sealed class DeadlineWaitTests
{
    [Fact]
    public void AWaitReturnsNearItsTimeout()
    {
        using var wake = new AutoResetEvent(false);
        using IDeadlineWait wait = DeadlineWait.ForThisPlatform(wake);

        long start = Stopwatch.GetTimestamp();
        wait.Wait(TimeSpan.FromMilliseconds(5));
        double elapsedMs = Stopwatch.GetElapsedTime(start).TotalMilliseconds;

        Assert.InRange(elapsedMs, 4, 200);
    }

    [Fact]
    public void AWakeEndsAWaitEarly()
    {
        using var wake = new AutoResetEvent(false);
        using IDeadlineWait wait = DeadlineWait.ForThisPlatform(wake);
        wake.Set();

        long start = Stopwatch.GetTimestamp();
        wait.Wait(TimeSpan.FromSeconds(10));
        double elapsedMs = Stopwatch.GetElapsedTime(start).TotalMilliseconds;

        Assert.InRange(elapsedMs, 0, 1000);
    }
}
