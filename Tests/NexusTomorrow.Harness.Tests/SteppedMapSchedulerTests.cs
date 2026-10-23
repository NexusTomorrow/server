using NexusTomorrow.Harness;
using NexusTomorrow.Maps.Scheduling;

namespace NexusTomorrow.Harness.Tests;

public sealed class SteppedMapSchedulerTests
{
    // A fault is reported once: the step that met it throws it, and the next step runs clean.
    [Fact]
    public async Task AMapThatThrowsOnItsTickFailsTheNextStepWithItsOwnException()
    {
        await using var scheduler = new SteppedMapScheduler();
        var thrower = new ThrowsOnFirstTick();
        var counter = new CountsTicks();
        scheduler.Register(thrower, MapPlacement.Pool);
        scheduler.Register(counter, MapPlacement.Pool);

        InvalidOperationException fault = await Assert.ThrowsAsync<InvalidOperationException>(() => scheduler.TickAsync());

        Assert.Equal("the map broke", fault.Message);
        Assert.Equal(1, counter.Ticks);

        await scheduler.TickAsync();
        Assert.Equal(2, counter.Ticks);
        Assert.Equal(2, thrower.Ticks);
    }

    private sealed class ThrowsOnFirstTick : ITickable
    {
        public int Ticks { get; private set; }

        public string Name => "thrower";

        public int TickMs => SteppedMapScheduler.StepMs;

        public void Tick(long nowMs)
        {
            Ticks++;
            if (Ticks == 1)
                throw new InvalidOperationException("the map broke");
        }
    }

    private sealed class CountsTicks : ITickable
    {
        public int Ticks { get; private set; }

        public string Name => "counter";

        public int TickMs => SteppedMapScheduler.StepMs;

        public void Tick(long nowMs) => Ticks++;
    }
}
