using System.Numerics;

namespace NexusTomorrow.Network.Codec.Tests;

public sealed class PackedFloatTests
{
    [Theory]
    [InlineData(1.0f, (ushort)0x3C00)]
    [InlineData(-2.0f, (ushort)0xC000)]
    [InlineData(0.5f, (ushort)0x3800)]
    public void PacksANormalValueByRebiasingItsExponent(float value, ushort expected)
    {
        // (magnitude - 0x37FFF000) >> 13 drops the exponent bias by 112 and adds half a unit before the shift:
        // 0x3F800000 gives 0x3C00, 0x40000000 gives 0x4000 under sign 0x8000, 0x3F000000 gives 0x3800.
        Assert.Equal(expected, PackedFloat.Pack(value));
    }

    [Fact]
    public void PacksPositiveZeroToZero()
    {
        Assert.Equal((ushort)0x0000, PackedFloat.Pack(0.0f));
    }

    [Fact]
    public void PacksNegativeZeroToTheSignBitAlone()
    {
        Assert.Equal((ushort)0x8000, PackedFloat.Pack(float.NegativeZero));
    }

    [Theory]
    [InlineData(5e-8f, (ushort)0x0000)]
    [InlineData(-5e-8f, (ushort)0x8000)]
    public void PacksAValueBelowTheSmallestSubnormalToItsSignAlone(float value, ushort expected)
    {
        // Below 2^-24 (0x33800000, about 5.96e-8) the pack keeps only the sign, where IEEE rounding would give 0x0001.
        Assert.Equal(expected, PackedFloat.Pack(value));
    }

    [Theory]
    [InlineData(1f / 16777216f, (ushort)0x0001)]
    [InlineData(1f / 32768f, (ushort)0x0200)]
    public void PacksASubnormalThroughTheShiftBranch(float value, ushort expected)
    {
        // 2^-24 is 0x33800000: ((0x800000 >> (113 - 103)) + 0x1000) >> 13 = 0x3000 >> 13 = 0x0001.
        // 2^-15 is 0x38000000: ((0x800000 >> (113 - 112)) + 0x1000) >> 13 = 0x401000 >> 13 = 0x0200.
        Assert.Equal(expected, PackedFloat.Pack(value));
    }

    [Fact]
    public void PacksTheSmallestNormalThroughTheRebiasBranch()
    {
        // 2^-14 is 0x38800000, past the shift branch's 0x387FEFFF limit: (0x38800000 - 0x37FFF000) >> 13 = 0x0400.
        Assert.Equal((ushort)0x0400, PackedFloat.Pack(1f / 16384f));
    }

    [Fact]
    public void RoundsAHalfwayMantissaAwayFromZero()
    {
        // 1 + 2^-11 is 0x3F801000, halfway between 0x3C00 and 0x3C01; the added 0x1000 carries it up, where
        // round-half-to-even would keep 0x3C00.
        Assert.Equal((ushort)0x3C01, PackedFloat.Pack(1.00048828125f));
    }

    [Theory]
    [InlineData(65536f, (ushort)0x7C00)]
    [InlineData(131008f, (ushort)0x7FFF)]
    public void MapsTheTopExponentToFiniteValuesBothWays(float value, ushort packed)
    {
        // Exponent 31 is an ordinary exponent here, not IEEE infinity or NaN. 2^16 is 0x47800000:
        // (0x47800000 - 0x37FFF000) >> 13 = 0x7C00. 131008 is 0x47FFE000, the largest packable value:
        // 0x0FFFF000 >> 13 = 0x7FFF. Unpack adds 0x1C000 and shifts left 13 to give the same bits back.
        Assert.Equal(packed, PackedFloat.Pack(value));
        Assert.Equal(value, PackedFloat.Unpack(packed));
    }

    [Theory]
    [InlineData(1e6f, (ushort)0x43FF)]
    [InlineData(-1e6f, (ushort)0xC3FF)]
    [InlineData(131072f, (ushort)0x43FF)]
    public void ClampsAValueAboveTheTopExponentToItsSignAnd0x43FF(float value, ushort expected)
    {
        // Every magnitude above 0x47FFEFFF clamps; 2^17 (0x48000000) is the first power of two past it.
        Assert.Equal(expected, PackedFloat.Pack(value));
    }

    [Theory]
    [InlineData((ushort)0x3C00, 1.0f)]
    [InlineData((ushort)0x3800, 0.5f)]
    [InlineData((ushort)0xC000, -2.0f)]
    public void UnpacksANormalValueByRebiasingItsExponent(ushort packed, float expected)
    {
        // (magnitude + 0x1C000) << 13 raises the exponent bias by 112: 0x3C00 gives 0x3F800000, 0x3800 gives
        // 0x3F000000, and 0x4000 gives 0x40000000 under sign 0x80000000.
        Assert.Equal(expected, PackedFloat.Unpack(packed));
    }

    [Theory]
    [InlineData((ushort)0x0001, 1f / 16777216f)]
    [InlineData((ushort)0x0200, 1f / 32768f)]
    public void UnpacksASubnormalByNormalizingItsMantissa(ushort packed, float expected)
    {
        // 0x0001: mantissa 0x2000 doubles ten times to 0x800000 while the exponent falls from 113 to 103,
        // giving 0x33800000 = 2^-24. 0x0200: 0x400000 doubles once, exponent 112, giving 0x38000000 = 2^-15.
        Assert.Equal(expected, PackedFloat.Unpack(packed));
    }

    [Fact]
    public void UnpacksSignedZerosWithTheirSign()
    {
        Assert.Equal(0x00000000u, BitConverter.SingleToUInt32Bits(PackedFloat.Unpack(0x0000)));
        Assert.Equal(0x80000000u, BitConverter.SingleToUInt32Bits(PackedFloat.Unpack(0x8000)));
    }

    [Fact]
    public void WritesAPackedFloatAsSixteenBits()
    {
        byte[] buffer = new byte[2];
        var writer = new BitWriter(buffer);

        writer.WritePackedFloat(1.0f);

        byte[] expected = [0x00, 0x3C];
        Assert.Equal(expected, buffer);
        Assert.Equal(16, writer.BitPosition);
    }

    [Fact]
    public void ReadsAPackedFloatFromSixteenBits()
    {
        byte[] body = [0x00, 0x38];
        var reader = new BitReader(body);

        Assert.Equal(0.5f, reader.ReadPackedFloat());
        Assert.Equal(16, reader.BitPosition);
    }

    [Fact]
    public void WritesAPackedVector3XThenYThenZ()
    {
        // 1.0f packs to 0x3C00, -2.0f to 0xC000 and 0.5f to 0x3800.
        byte[] buffer = new byte[6];
        var writer = new BitWriter(buffer);

        writer.WritePackedVector3(new Vector3(1.0f, -2.0f, 0.5f));

        byte[] expected = [0x00, 0x3C, 0x00, 0xC0, 0x00, 0x38];
        Assert.Equal(expected, buffer);
        Assert.Equal(48, writer.BitPosition);
    }

    [Fact]
    public void ReadsAPackedVector3XThenYThenZ()
    {
        byte[] body = [0x00, 0x3C, 0x00, 0xC0, 0x00, 0x38];
        var reader = new BitReader(body);

        Assert.Equal(new Vector3(1.0f, -2.0f, 0.5f), reader.ReadPackedVector3());
        Assert.Equal(48, reader.BitPosition);
    }
}
