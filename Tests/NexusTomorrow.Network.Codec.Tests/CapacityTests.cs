using NexusTomorrow.Network.Codec.Tests.Fixtures;
using NexusTomorrow.Network.World.Message.Model.Hazard;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;

namespace NexusTomorrow.Network.Codec.Tests;

public sealed class CapacityTests
{
    private static byte[] Write(IWireStruct value)
    {
        byte[] buffer = new byte[(value.MeasureBits() + 7) / 8];
        var writer = new BitWriter(buffer);
        value.Write(ref writer);
        return buffer;
    }

    [Fact]
    public void ACapacityRefusesAWritePastIt()
    {
        var fixture = new CapacityFixture();
        fixture.Values.AddRange([1, 2, 3, 4]);

        CodecException refused = Assert.Throws<CodecException>(() => Write(fixture));

        Assert.Equal("CapacityFixture.Values holds 4 elements; the client holds at most 3.", refused.Message);
    }

    [Fact]
    public void ACapacityAllowsAWriteAtIt()
    {
        var fixture = new CapacityFixture();
        fixture.Values.AddRange([1, 2, 3]);

        byte[] bytes = Write(fixture);

        var read = new CapacityFixture();
        var reader = new BitReader(bytes);
        read.Read(ref reader);
        byte[] expected = [1, 2, 3];
        Assert.Equal(expected, read.Values);
    }

    [Fact]
    public void HazardModifiersHoldAtMost260()
    {
        var message = new HazardMaxModifiers();
        message.HazardModifiers.AddRange(Enumerable.Range(0, 260).Select(_ => new HazardMaxModifier()));
        Write(message);

        message.HazardModifiers.Add(new HazardMaxModifier());

        CodecException refused = Assert.Throws<CodecException>(() => Write(message));
        Assert.Contains("at most 260", refused.Message);
    }

    [Fact]
    public void HazardsSecondListHoldsAtMost5()
    {
        var message = new HazardMaxModifiers();
        message.Unknown1.AddRange(Enumerable.Range(0, 5).Select(_ => new HazardMaxModifier()));
        Write(message);

        message.Unknown1.Add(new HazardMaxModifier());

        CodecException refused = Assert.Throws<CodecException>(() => Write(message));
        Assert.Contains("at most 5", refused.Message);
    }

    [Fact]
    public void CachedAugmentsHoldAtMost54()
    {
        var message = new PlayerState();
        message.CachedAugments.AddRange(Enumerable.Range(0, 54).Select(i => (ushort)i));
        Write(message);

        message.CachedAugments.Add(54);

        CodecException refused = Assert.Throws<CodecException>(() => Write(message));
        Assert.Contains("at most 54", refused.Message);
    }
}
