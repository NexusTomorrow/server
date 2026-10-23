using NexusTomorrow.Maps.Timing;

namespace NexusTomorrow.Maps.Tests.Timing;

public sealed class TimerWheelTests
{
    [Fact]
    public void ATimerFiresAtItsDueMillisecondAndNotBefore()
    {
        var wheel = new TimerWheel<int>(startMs: 0);
        var handler = new Recorder();
        wheel.Schedule(30, 1);

        Assert.Equal(0, wheel.Advance(29, handler));
        Assert.Equal(1, wheel.Advance(30, handler));

        Assert.Equal([(1, 30L)], handler.Fired);
    }

    [Fact]
    public void TimersFireEarliestFirstAndInScheduleOrderWithinAMillisecond()
    {
        var wheel = new TimerWheel<int>(startMs: 0);
        var handler = new Recorder();
        wheel.Schedule(10, 1);
        wheel.Schedule(5, 2);
        wheel.Schedule(10, 3);

        wheel.Advance(25, handler);

        int[] expected = [2, 1, 3];
        Assert.Equal(expected, handler.Fired.Select(f => f.Payload).ToArray());
    }

    [Fact]
    public void ATimerBeyondOneTurnOfTheWheelFiresAtItsOwnTime()
    {
        var wheel = new TimerWheel<int>(startMs: 0);
        var handler = new Recorder();
        wheel.Schedule(3000, 7);

        for (long now = 25; now <= 2975; now += 25)
            wheel.Advance(now, handler);
        Assert.Empty(handler.Fired);

        wheel.Advance(3000, handler);
        Assert.Equal([(7, 3000L)], handler.Fired);
    }

    [Fact]
    public void ACancelledTimerNeverFiresAndCancelsOnce()
    {
        var wheel = new TimerWheel<int>(startMs: 0);
        var handler = new Recorder();
        TimerHandle handle = wheel.Schedule(10, 1);

        Assert.True(wheel.Cancel(handle));
        Assert.False(wheel.Cancel(handle));
        wheel.Advance(100, handler);

        Assert.Empty(handler.Fired);
        Assert.Equal(0, wheel.Count);
    }

    [Fact]
    public void AStaleHandleCancelsNothingOnceItsSlotIsReused()
    {
        var wheel = new TimerWheel<int>(startMs: 0);
        var handler = new Recorder();
        TimerHandle first = wheel.Schedule(10, 1);
        wheel.Advance(10, handler);
        wheel.Schedule(20, 2);

        Assert.False(wheel.Cancel(first));
        wheel.Advance(20, handler);

        Assert.Equal([(1, 10L), (2, 20L)], handler.Fired);
    }

    [Fact]
    public void ATimeAlreadyPassedFiresOnTheNextAdvance()
    {
        var wheel = new TimerWheel<int>(startMs: 0);
        var handler = new Recorder();
        wheel.Advance(100, handler);
        wheel.Schedule(50, 1);

        wheel.Advance(101, handler);

        Assert.Equal([(1, 101L)], handler.Fired);
    }

    [Fact]
    public void AHandlerCanScheduleTheNextTimerOfAChain()
    {
        var wheel = new TimerWheel<int>(startMs: 0);
        var handler = new Chain(wheel);
        wheel.Schedule(10, 0);

        wheel.Advance(45, handler);

        Assert.Equal([10L, 20L, 30L, 40L], handler.Dues);
    }

    [Fact]
    public void AnEmptyWheelSkipsStraightToTheNewTime()
    {
        var wheel = new TimerWheel<int>(startMs: 0);
        var handler = new Recorder();

        Assert.Equal(0, wheel.Advance(1_000_000_000, handler));
        Assert.Equal(1_000_000_000L, wheel.LastMs);

        wheel.Schedule(1_000_000_005, 1);
        wheel.Advance(1_000_000_005, handler);
        Assert.Equal([(1, 1_000_000_005L)], handler.Fired);
    }

    [Fact]
    public void AHandlerThatThrowsLeavesTheRestDueForTheNextAdvance()
    {
        var wheel = new TimerWheel<int>(startMs: 0);
        var handler = new ThrowsOnce();
        wheel.Schedule(10, 1);
        wheel.Schedule(10, 2);

        Assert.Throws<InvalidOperationException>(() => wheel.Advance(10, handler));
        wheel.Advance(10, handler);

        int[] expected = [2];
        Assert.Equal(expected, handler.Payloads);
    }

    [Fact]
    public void SchedulingAndFiringAllocateNothingOnceTheWheelHasGrown()
    {
        var wheel = new TimerWheel<int>(startMs: 0, capacity: 16);
        var handler = new Counter();
        for (int i = 1; i <= 100; i++)
            wheel.Schedule(i, i);
        wheel.Advance(200, handler);

        long now = 200;
        long before = GC.GetAllocatedBytesForCurrentThread();
        for (int round = 0; round < 100; round++)
        {
            for (int i = 0; i < 100; i++)
                wheel.Schedule(now + 1 + (i % 25), i);
            now += 25;
            wheel.Advance(now, handler);
        }

        long allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(0L, allocated);
        Assert.Equal(10_100, handler.Count);
    }

    [Fact]
    public void AWheelGrownPastItsStartingCapacityFiresEveryTimerInOrderAndCancelsAcrossTheGrowth()
    {
        var wheel = new TimerWheel<int>(startMs: 0, capacity: 1);
        var handler = new Recorder();
        var handles = new TimerHandle[100];
        for (int i = 0; i < 100; i++)
            handles[i] = wheel.Schedule(1 + ((i % 10) * 700), i);

        // 0 and 1 were scheduled before the array first grew, 5 before its later growths, 70 and 99 after the last.
        int[] cancelled = [0, 1, 5, 70, 99];
        foreach (int i in cancelled)
            Assert.True(wheel.Cancel(handles[i]));
        Assert.Equal(95, wheel.Count);

        Assert.Equal(95, wheel.Advance(10_000, handler));

        List<(int, long)> expected = [];
        for (int k = 0; k < 10; k++)
        {
            for (int j = 0; j < 10; j++)
            {
                int i = (j * 10) + k;
                if (!cancelled.Contains(i))
                    expected.Add((i, 1 + (k * 700L)));
            }
        }

        Assert.Equal(expected, handler.Fired);
        Assert.Equal(0, wheel.Count);
    }
    private sealed class Recorder : ITimerHandler<int>
    {
        public List<(int Payload, long DueMs)> Fired { get; } = [];

        public void OnTimer(in int payload, long dueMs) => Fired.Add((payload, dueMs));
    }

    private sealed class Chain(TimerWheel<int> wheel) : ITimerHandler<int>
    {
        public List<long> Dues { get; } = [];

        public void OnTimer(in int payload, long dueMs)
        {
            Dues.Add(dueMs);
            wheel.Schedule(dueMs + 10, 0);
        }
    }

    private sealed class ThrowsOnce : ITimerHandler<int>
    {
        private bool _thrown;

        public List<int> Payloads { get; } = [];

        public void OnTimer(in int payload, long dueMs)
        {
            if (!_thrown)
            {
                _thrown = true;
                throw new InvalidOperationException();
            }

            Payloads.Add(payload);
        }
    }

    private sealed class Counter : ITimerHandler<int>
    {
        public int Count { get; private set; }

        public void OnTimer(in int payload, long dueMs) => Count++;
    }
}
