using System.Numerics;

namespace NexusTomorrow.Network.Codec.Tests;

public sealed class BitReaderTests
{
    [Fact]
    public void ReadsThreeBitsThenAByteAcrossTheBoundary()
    {
        // Byte 0 is 0b1111_1101 and byte 1 is 0b0000_0111: bits 0 to 2 hold 0b101 and bits 3 to 10 hold 0xFF.
        byte[] body = [0xFD, 0x07];
        var reader = new BitReader(body);

        Assert.Equal(0b101UL, reader.ReadBits(3));
        Assert.Equal(0xFFUL, reader.ReadBits(8));
        Assert.Equal(11, reader.BitPosition);
        Assert.Equal(5, reader.BitsRemaining);
    }

    [Fact]
    public void ReadsASixtyFourBitValueLeastSignificantByteFirst()
    {
        byte[] body = [0xEF, 0xCD, 0xAB, 0x89, 0x67, 0x45, 0x23, 0x01];
        var reader = new BitReader(body);

        Assert.Equal(0x0123456789ABCDEFUL, reader.ReadBits(64));
        Assert.Equal(64, reader.BitPosition);
        Assert.Equal(0, reader.BitsRemaining);
    }

    [Fact]
    public void ReadsAValueFromMidByteAcrossThreeBytes()
    {
        // Bits 4 to 19 hold 0xABCD low nibble first: D in byte 0's high half, C and B in byte 1, A in byte 2's low half.
        byte[] body = [0xD6, 0xBC, 0x0A];
        var reader = new BitReader(body);

        Assert.Equal(0b0110UL, reader.ReadBits(4));
        Assert.Equal(0xABCDUL, reader.ReadBits(16));
        Assert.Equal(20, reader.BitPosition);
        Assert.Equal(4, reader.BitsRemaining);
    }

    [Fact]
    public void ReadsSingleBitsFromBitZeroUpward()
    {
        // Byte 0 = 0b0100_1101 gives 1 0 1 1 0 0 1 0 from bit 0; byte 1's bit 0 is the ninth.
        byte[] body = [0x4D, 0x01];
        var reader = new BitReader(body);

        ulong[] expected = [1, 0, 1, 1, 0, 0, 1, 0, 1];
        foreach (ulong bit in expected)
            Assert.Equal(bit, reader.ReadBits(1));

        Assert.Equal(9, reader.BitPosition);
        Assert.Equal(7, reader.BitsRemaining);
    }

    [Theory]
    [InlineData(8, 1)]
    [InlineData(5, 4)]
    public void ThrowsWhenAReadRunsPastTheEnd(int firstBits, int secondBits)
    {
        Assert.Throws<CodecException>(() =>
        {
            var reader = new BitReader(new byte[1]);
            reader.ReadBits(firstBits);
            reader.ReadBits(secondBits);
        });
    }

    [Fact]
    public void ThrowsWhenOneReadIsWiderThanTheBody()
    {
        Assert.Throws<CodecException>(() => new BitReader(new byte[1]).ReadBits(9));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(65)]
    public void ThrowsForAWidthOutsideOneToSixtyFour(int bits)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BitReader(new byte[16]).ReadBits(bits));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BitReader(new byte[16]).ReadSignedBits(bits));
    }

    [Theory]
    [InlineData((byte)0b111, -1L)]
    [InlineData((byte)0b100, -4L)]
    [InlineData((byte)0b011, 3L)]
    public void SignExtendsFromTheTopBitOfTheField(byte raw, long expected)
    {
        byte[] body = [raw];
        var reader = new BitReader(body);

        Assert.Equal(expected, reader.ReadSignedBits(3));
        Assert.Equal(3, reader.BitPosition);
    }

    [Fact]
    public void ReadsTheMostNegativeSixtyFourBitValue()
    {
        byte[] body = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80];
        var reader = new BitReader(body);

        Assert.Equal(long.MinValue, reader.ReadSignedBits(64));
    }

    [Fact]
    public void ReadsASingleFromItsIeeeBits()
    {
        // 0x3F800000 is 1.0f.
        byte[] body = [0x00, 0x00, 0x80, 0x3F];
        var reader = new BitReader(body);

        Assert.Equal(1.0f, reader.ReadFloat32());
        Assert.Equal(32, reader.BitPosition);
    }

    [Fact]
    public void ReadsADoubleFromItsIeeeBits()
    {
        // 0xC004000000000000 is sign 1, exponent 0x400, mantissa 0x4000000000000: -1.25 x 2^1 = -2.5.
        byte[] body = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0xC0];
        var reader = new BitReader(body);

        Assert.Equal(-2.5, reader.ReadFloat64());
        Assert.Equal(64, reader.BitPosition);
    }

    [Fact]
    public void ReadsBackADoubleTheWriterWroteMidByte()
    {
        // 3 bits then 64 bits: 67 bits in 9 bytes.
        byte[] buffer = new byte[9];
        var writer = new BitWriter(buffer);
        writer.WriteBits(0b101, 3);
        writer.WriteFloat64(Math.PI);

        var reader = new BitReader(buffer);

        Assert.Equal(0b101UL, reader.ReadBits(3));
        Assert.Equal(Math.PI, reader.ReadFloat64());
        Assert.Equal(67, reader.BitPosition);
    }

    [Fact]
    public void ReadsAVector3XThenYThenZ()
    {
        // 0x3F800000 is 1.0f, 0x40000000 is 2.0f and 0xBF000000 is -0.5f.
        byte[] body = [0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0xBF];
        var reader = new BitReader(body);

        Assert.Equal(new Vector3(1.0f, 2.0f, -0.5f), reader.ReadVector3());
        Assert.Equal(96, reader.BitPosition);
    }

    [Fact]
    public void ReadsBytesAtAnUnalignedPosition()
    {
        // Bits 0 to 3 hold 0xA, bits 4 to 11 hold 0x12 and bits 12 to 19 hold 0x34, each low nibble first.
        byte[] body = [0x2A, 0x41, 0x03];
        var reader = new BitReader(body);
        byte[] destination = new byte[2];

        Assert.Equal(0xAUL, reader.ReadBits(4));
        reader.ReadBytes(destination);

        byte[] expected = [0x12, 0x34];
        Assert.Equal(expected, destination);
        Assert.Equal(20, reader.BitPosition);
        Assert.Equal(4, reader.BitsRemaining);
    }

    [Fact]
    public void ThrowsWhenReadBytesRunsPastTheEnd()
    {
        Assert.Throws<CodecException>(() =>
        {
            var reader = new BitReader(new byte[1]);
            reader.ReadBits(1);
            reader.ReadBytes(new byte[1]);
        });
    }

    [Fact]
    public void AlignToByteSkipsToTheNextBoundary()
    {
        byte[] body = [0xFF, 0x5A];
        var reader = new BitReader(body);

        reader.ReadBits(3);
        reader.AlignToByte();

        Assert.Equal(8, reader.BitPosition);
        Assert.Equal(8, reader.BitsRemaining);
        Assert.Equal(0x5AUL, reader.ReadBits(8));
    }

    [Fact]
    public void AlignToByteOnABoundaryLeavesThePositionAlone()
    {
        var reader = new BitReader(new byte[2]);

        reader.AlignToByte();
        Assert.Equal(0, reader.BitPosition);
        Assert.Equal(16, reader.BitsRemaining);

        reader.ReadBits(8);
        reader.AlignToByte();
        Assert.Equal(8, reader.BitPosition);
        Assert.Equal(8, reader.BitsRemaining);
    }

    [Fact]
    public void BitPositionAndBitsRemainingAdvanceByEachKindOfRead()
    {
        // A 40-byte body of zeros is 320 bits; a zero string header or count reads as an empty string.
        var reader = new BitReader(new byte[40]);
        byte[] destination = new byte[2];
        AssertPosition(reader, 0, 320);

        reader.ReadBits(1);
        AssertPosition(reader, 1, 319);

        reader.ReadSignedBits(5);
        AssertPosition(reader, 6, 314);

        reader.ReadFloat32();
        AssertPosition(reader, 38, 282);

        reader.ReadFloat64();
        AssertPosition(reader, 102, 218);

        reader.ReadPackedFloat();
        AssertPosition(reader, 118, 202);

        reader.ReadVector3();
        AssertPosition(reader, 214, 106);

        reader.ReadPackedVector3();
        AssertPosition(reader, 262, 58);

        // A 1-bit flag and a 7-bit length.
        reader.ReadWideString();
        AssertPosition(reader, 270, 50);

        reader.ReadNarrowString();
        AssertPosition(reader, 278, 42);

        // A 16-bit count.
        reader.ReadTerminatedWideString();
        AssertPosition(reader, 294, 26);

        reader.ReadBytes(destination);
        AssertPosition(reader, 310, 10);

        reader.AlignToByte();
        AssertPosition(reader, 312, 8);
    }

    private static void AssertPosition(BitReader reader, int bitPosition, int bitsRemaining)
    {
        Assert.Equal(bitPosition, reader.BitPosition);
        Assert.Equal(bitsRemaining, reader.BitsRemaining);
    }
}
