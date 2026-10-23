using NexusTomorrow.Maps.Scheduling;

namespace NexusTomorrow.Maps.Tests.Scheduling;

public sealed class DeadlineQueueTests
{
    [Fact]
    public void TheEarliestDueMapIsTakenFirst()
    {
        var queue = new DeadlineQueue();
        var late = new ScheduledMap(new IdleMap(), 40);
        var early = new ScheduledMap(new IdleMap(), 10);
        queue.Add(late);
        queue.Add(early);

        Assert.Same(early, queue.TakeDue(50, out long firstWait));
        Assert.Equal(0L, firstWait);
        Assert.Same(late, queue.TakeDue(50, out long secondWait));
        Assert.Equal(0L, secondWait);
        Assert.Null(queue.TakeDue(50, out long emptyWait));
        Assert.Equal(-1L, emptyWait);
    }

    [Fact]
    public void NothingIsTakenBeforeItsDeadlineAndTheWaitIsTheTimeLeft()
    {
        var queue = new DeadlineQueue();
        queue.Add(new ScheduledMap(new IdleMap(), 100));

        Assert.Null(queue.TakeDue(70, out long waitMs));
        Assert.Equal(30L, waitMs);
        Assert.Equal(1, queue.Count);
    }

    [Fact]
    public void ARemovedMapIsPassedOver()
    {
        var queue = new DeadlineQueue();
        var removed = new ScheduledMap(new IdleMap(), 10) { Removed = true };
        var kept = new ScheduledMap(new IdleMap(), 20);
        queue.Add(removed);
        queue.Add(kept);

        Assert.Same(kept, queue.TakeDue(30, out _));
        Assert.Equal(0, queue.Count);
    }

    [Theory]
    [InlineData(100, 25, 110, 125, 0)]
    [InlineData(100, 25, 100, 125, 0)]
    [InlineData(100, 25, 125, 125, 0)]
    [InlineData(100, 25, 126, 150, 1)]
    [InlineData(100, 25, 200, 200, 3)]
    [InlineData(100, 25, 90, 125, 0)]
    public void AMissedDeadlineIsSkippedNotReplayed(long deadline, int tickMs, long now, long expectedNext, long expectedSkipped)
    {
        long next = DeadlineQueue.NextDeadline(deadline, tickMs, now, out long skipped);

        Assert.Equal(expectedNext, next);
        Assert.Equal(expectedSkipped, skipped);
    }

    private sealed class IdleMap : ITickable
    {
        public string Name => "idle";

        public int TickMs => 25;

        public void Tick(long nowMs)
        {
        }
    }
}
