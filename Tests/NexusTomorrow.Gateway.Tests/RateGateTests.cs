using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Gateway.Catalog;

namespace NexusTomorrow.Gateway.Tests;

public sealed class RateGateTests
{
    private static readonly OpcodeRow s_row = new(0x07E0, MessageOwner.Realm, MessagePriority.Normal, PerSecond: 2, OverLimitAction.Drop);

    [Fact]
    public void ARowPassesItsRateAndRefusesTheNextInTheSameSecond()
    {
        var time = new FakeTimeProvider();
        var gate = new RateGate(time);
        Assert.True(gate.TryPass(s_row));
        Assert.True(gate.TryPass(s_row));
        Assert.False(gate.TryPass(s_row));
    }

    [Fact]
    public void TheAllowanceRefillsOverTime()
    {
        var time = new FakeTimeProvider();
        var gate = new RateGate(time);
        gate.TryPass(s_row);
        gate.TryPass(s_row);
        time.Advance(TimeSpan.FromMilliseconds(500));
        Assert.True(gate.TryPass(s_row));
        Assert.False(gate.TryPass(s_row));
    }
}
