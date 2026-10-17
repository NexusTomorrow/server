using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;

namespace NexusTomorrow.Owners.Tests;

public sealed class OwnerWatchdogTests
{
    [Fact]
    public void AnOwnerThatStopsDrainingIsReportedAfterTheStallTime()
    {
        var time = new FakeTimeProvider();
        var owner = new StubOwner { QueueLength = 3, Handled = 10 };
        var watchdog = new OwnerWatchdog([owner], time, TimeSpan.FromSeconds(10), NullLogger<OwnerWatchdog>.Instance);

        Assert.Empty(watchdog.Check());
        time.Advance(TimeSpan.FromSeconds(9));
        Assert.Empty(watchdog.Check());
        time.Advance(TimeSpan.FromSeconds(1));
        Assert.Equal(["stub"], watchdog.Check());
    }

    [Fact]
    public void AnOwnerThatKeepsDrainingIsNeverReported()
    {
        var time = new FakeTimeProvider();
        var owner = new StubOwner { QueueLength = 3, Handled = 10 };
        var watchdog = new OwnerWatchdog([owner], time, TimeSpan.FromSeconds(10), NullLogger<OwnerWatchdog>.Instance);
        for (int i = 0; i < 5; i++)
        {
            watchdog.Check();
            time.Advance(TimeSpan.FromSeconds(5));
            owner.Handled++;
        }

        Assert.Empty(watchdog.Check());
    }

    [Fact]
    public void AnIdleOwnerIsNeverReported()
    {
        var time = new FakeTimeProvider();
        var owner = new StubOwner { QueueLength = 0, Handled = 10 };
        var watchdog = new OwnerWatchdog([owner], time, TimeSpan.FromSeconds(10), NullLogger<OwnerWatchdog>.Instance);
        watchdog.Check();
        time.Advance(TimeSpan.FromMinutes(5));
        Assert.Empty(watchdog.Check());
    }

    private sealed class StubOwner : IOwner
    {
        public string Name => "stub";

        public int QueueLength { get; set; }

        public long Handled { get; set; }
    }
}
