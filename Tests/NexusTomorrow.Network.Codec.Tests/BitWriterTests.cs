using System.Numerics;

namespace NexusTomorrow.Network.Codec.Tests;

public sealed class BitWriterTests
{
    [Fact]
    public void WritesThreeBitsThenAByteAcrossTheBoundary()
    {
        // 0b101 fills bits 0 to 2 and 0xFF bits 3 to 10: byte 0 is 0b1111_1101, byte 1 is 0b0000_0111.
        byte[] buffer = new byte[2];
        var writer = new BitWriter(buffer);

        writer.WriteBits(0b101, 3);
        writer.WriteBits(0xFF, 8);

        byte[] expected = [0xFD, 0x07];
        Assert.Equal(expected, buffer);
        Assert.Equal(11, writer.BitPosition);
        Assert.Equal(2, writer.BytesWritten);
    }

    [Fact]
    public void WritesASixtyFourBitValueLeastSignificantByteFirst()
    {
        byte[] buffer = new byte[8];
        var writer = new BitWriter(buffer);

        writer.WriteBits(0x0123456789ABCDEF, 64);

        byte[] expected = [0xEF, 0xCD, 0xAB, 0x89, 0x67, 0x45, 0x23, 0x01];
        Assert.Equal(expected, buffer);
        Assert.Equal(64, writer.BitPosition);
        Assert.Equal(8, writer.BytesWritten);
    }

    [Fact]
    public void WritesAValueFromMidByteAcrossThreeBytes()
    {
        // 0xABCD lands on bits 4 to 19 low nibble first: D fills byte 0's high half, C and B fill byte 1, A fills
        // byte 2's low half.
        byte[] buffer = new byte[3];
        var writer = new BitWriter(buffer);

        writer.WriteBits(0b0110, 4);
        writer.WriteBits(0xABCD, 16);

        byte[] expected = [0xD6, 0xBC, 0x0A];
        Assert.Equal(expected, buffer);
        Assert.Equal(20, writer.BitPosition);
        Assert.Equal(3, writer.BytesWritten);
    }

    [Fact]
    public void WritesNineSingleBitsFromBitZeroUpward()
    {
        // The first eight bits, 1 0 1 1 0 0 1 0 from bit 0, make byte 0 = 0b0100_1101; the ninth is byte 1's bit 0.
        byte[] buffer = new byte[2];
        var writer = new BitWriter(buffer);

        ulong[] bits = [1, 0, 1, 1, 0, 0, 1, 0, 1];
        foreach (ulong bit in bits)
            writer.WriteBits(bit, 1);

        byte[] expected = [0x4D, 0x01];
        Assert.Equal(expected, buffer);
        Assert.Equal(9, writer.BitPosition);
        Assert.Equal(2, writer.BytesWritten);
    }

    [Fact]
    public void ClearsEachByteWhenItFirstTouchesIt()
    {
        // Bits 0 to 10 touch bytes 0 and 1, so their stale 0xFF is cleared; byte 2 is never touched and keeps it.
        byte[] buffer = [0xFF, 0xFF, 0xFF];
        var writer = new BitWriter(buffer);

        writer.WriteBits(0b001, 3);
        writer.WriteBits(0x00, 8);

        byte[] expected = [0x01, 0x00, 0xFF];
        Assert.Equal(expected, buffer);
    }

    [Theory]
    [InlineData(8UL, 3)]
    [InlineData(256UL, 8)]
    [InlineData(0x8000000000000000UL, 63)]
    public void ThrowsWhenTheValueDoesNotFitTheWidth(ulong value, int bits)
    {
        Assert.Throws<CodecException>(() => new BitWriter(new byte[8]).WriteBits(value, bits));
    }

    [Theory]
    [InlineData(8, 1)]
    [InlineData(5, 4)]
    public void ThrowsWhenTheBufferIsFull(int firstBits, int secondBits)
    {
        Assert.Throws<CodecException>(() =>
        {
            var writer = new BitWriter(new byte[1]);
            writer.WriteBits(0, firstBits);
            writer.WriteBits(0, secondBits);
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(65)]
    public void ThrowsForAWidthOutsideOneToSixtyFour(int bits)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BitWriter(new byte[16]).WriteBits(0, bits));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BitWriter(new byte[16]).WriteSignedBits(0, bits));
    }

    [Theory]
    [InlineData(-1L, (byte)0b111)]
    [InlineData(-4L, (byte)0b100)]
    [InlineData(3L, (byte)0b011)]
    public void WritesASignedValueAsTwosComplement(long value, byte expected)
    {
        byte[] buffer = new byte[1];
        var writer = new BitWriter(buffer);

        writer.WriteSignedBits(value, 3);

        Assert.Equal(expected, buffer[0]);
        Assert.Equal(3, writer.BitPosition);
    }

    [Fact]
    public void WritesTheMostNegativeSixtyFourBitValue()
    {
        byte[] buffer = new byte[8];
        var writer = new BitWriter(buffer);

        writer.WriteSignedBits(long.MinValue, 64);

        byte[] expected = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80];
        Assert.Equal(expected, buffer);
    }

    [Theory]
    [InlineData(-5L)]
    [InlineData(4L)]
    public void ThrowsWhenASignedValueDoesNotFitTheWidth(long value)
    {
        // A signed 3-bit field holds -4 to 3.
        Assert.Throws<CodecException>(() => new BitWriter(new byte[1]).WriteSignedBits(value, 3));
    }

    [Fact]
    public void WritesASingleAsItsIeeeBits()
    {
        // 1.0f is 0x3F800000.
        byte[] buffer = new byte[4];
        var writer = new BitWriter(buffer);

        writer.WriteFloat32(1.0f);

        byte[] expected = [0x00, 0x00, 0x80, 0x3F];
        Assert.Equal(expected, buffer);
        Assert.Equal(32, writer.BitPosition);
    }

    [Fact]
    public void WritesADoubleAsItsIeeeBits()
    {
        // -2.5 is -1.25 x 2^1: sign 1, exponent 0x400, mantissa 0x4000000000000, so 0xC004000000000000.
        byte[] buffer = new byte[8];
        var writer = new BitWriter(buffer);

        writer.WriteFloat64(-2.5);

        byte[] expected = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0xC0];
        Assert.Equal(expected, buffer);
        Assert.Equal(64, writer.BitPosition);
    }

    [Fact]
    public void WritesAVector3XThenYThenZ()
    {
        // 1.0f is 0x3F800000, 2.0f is 0x40000000 and -0.5f is 0xBF000000.
        byte[] buffer = new byte[12];
        var writer = new BitWriter(buffer);

        writer.WriteVector3(new Vector3(1.0f, 2.0f, -0.5f));

        byte[] expected = [0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0xBF];
        Assert.Equal(expected, buffer);
        Assert.Equal(96, writer.BitPosition);
    }

    [Fact]
    public void WritesBytesAtAnUnalignedPosition()
    {
        // 0xA fills bits 0 to 3, 0x12 bits 4 to 11 and 0x34 bits 12 to 19, each low nibble first.
        byte[] buffer = new byte[3];
        var writer = new BitWriter(buffer);
        byte[] source = [0x12, 0x34];

        writer.WriteBits(0xA, 4);
        writer.WriteBytes(source);

        byte[] expected = [0x2A, 0x41, 0x03];
        Assert.Equal(expected, buffer);
        Assert.Equal(20, writer.BitPosition);
        Assert.Equal(3, writer.BytesWritten);
    }

    [Fact]
    public void AlignToByteSkipsToTheNextBoundaryLeavingZeroBits()
    {
        // Byte 0 was cleared when bit 0 was written, so bits 3 to 7 read zero over the stale 0xFF.
        byte[] buffer = [0xFF, 0xFF];
        var writer = new BitWriter(buffer);

        writer.WriteBits(0b101, 3);
        writer.AlignToByte();

        Assert.Equal(8, writer.BitPosition);
        Assert.Equal(1, writer.BytesWritten);

        writer.WriteBits(0xAA, 8);

        byte[] expected = [0x05, 0xAA];
        Assert.Equal(expected, buffer);
    }

    [Fact]
    public void AlignToByteOnABoundaryLeavesThePositionAlone()
    {
        var writer = new BitWriter(new byte[2]);

        writer.AlignToByte();
        Assert.Equal(0, writer.BitPosition);
        Assert.Equal(0, writer.BytesWritten);

        writer.WriteBits(0xFF, 8);
        writer.AlignToByte();
        Assert.Equal(8, writer.BitPosition);
        Assert.Equal(1, writer.BytesWritten);
    }

    [Fact]
    public void BitPositionAndBytesWrittenAdvanceByEachKindOfWrite()
    {
        // BytesWritten counts a partly written last byte: (BitPosition + 7) / 8. The writes total 416 bits, 52 bytes.
        var writer = new BitWriter(new byte[52]);
        byte[] source = [0x01, 0x02, 0x03];
        AssertPosition(writer, 0, 0);

        writer.WriteBits(1, 1);
        AssertPosition(writer, 1, 1);

        writer.WriteSignedBits(-3, 5);
        AssertPosition(writer, 6, 1);

        writer.WriteFloat32(1.0f);
        AssertPosition(writer, 38, 5);

        writer.WriteFloat64(1.0);
        AssertPosition(writer, 102, 13);

        writer.WritePackedFloat(1.0f);
        AssertPosition(writer, 118, 15);

        writer.WriteVector3(Vector3.One);
        AssertPosition(writer, 214, 27);

        writer.WritePackedVector3(Vector3.One);
        AssertPosition(writer, 262, 33);

        // A 1-bit flag, a 7-bit length and two 16-bit units.
        writer.WriteWideString("AB");
        AssertPosition(writer, 302, 38);

        // A 1-bit flag, a 7-bit length and two bytes.
        writer.WriteNarrowString("AB");
        AssertPosition(writer, 326, 41);

        // A 16-bit count, two 16-bit units and a 16-bit terminator.
        writer.WriteTerminatedWideString("AB");
        AssertPosition(writer, 390, 49);

        writer.WriteBytes(source);
        AssertPosition(writer, 414, 52);

        writer.AlignToByte();
        AssertPosition(writer, 416, 52);
    }

    private static void AssertPosition(BitWriter writer, int bitPosition, int bytesWritten)
    {
        Assert.Equal(bitPosition, writer.BitPosition);
        Assert.Equal(bytesWritten, writer.BytesWritten);
    }
}
