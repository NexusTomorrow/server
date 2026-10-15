namespace NexusTomorrow.Network.Codec.Tests;

public sealed class StringTests
{
    [Fact]
    public void WritesAnEmptyWideStringAsEightZeroBits()
    {
        // Flag 0 and length 0; the stale 0xFF shows the bits are written, not skipped.
        byte[] buffer = [0xFF];
        var writer = new BitWriter(buffer);

        writer.WriteWideString(string.Empty);

        byte[] expected = [0x00];
        Assert.Equal(expected, buffer);
        Assert.Equal(8, writer.BitPosition);
    }

    [Fact]
    public void WritesAWideStringAsFlagLengthThenSixteenBitUnits()
    {
        // Flag 0 in bit 0 and length 2 in bits 1 to 7 make byte 0 = 0b0000_0100; 'A' and 'B' follow as 0x0041, 0x0042.
        byte[] buffer = new byte[5];
        var writer = new BitWriter(buffer);

        writer.WriteWideString("AB");

        byte[] expected = [0x04, 0x41, 0x00, 0x42, 0x00];
        Assert.Equal(expected, buffer);
        Assert.Equal(40, writer.BitPosition);
    }

    [Fact]
    public void WritesAWideUnitAboveOneByteAsSixteenBits()
    {
        // U+20AC writes low byte 0xAC then high byte 0x20.
        byte[] buffer = new byte[3];
        var writer = new BitWriter(buffer);

        writer.WriteWideString("€");

        byte[] expected = [0x02, 0xAC, 0x20];
        Assert.Equal(expected, buffer);
    }

    [Fact]
    public void ReadsAWideStringAsFlagLengthThenSixteenBitUnits()
    {
        byte[] body = [0x04, 0x41, 0x00, 0x42, 0x00];
        var reader = new BitReader(body);

        Assert.Equal("AB", reader.ReadWideString());
        Assert.Equal(40, reader.BitPosition);
        Assert.Equal(0, reader.BitsRemaining);
    }

    [Fact]
    public void ReadsAnEmptyWideStringFromEightZeroBits()
    {
        byte[] body = [0x00];
        var reader = new BitReader(body);

        Assert.Empty(reader.ReadWideString());
        Assert.Equal(8, reader.BitPosition);
    }

    [Fact]
    public void WritesA127CharacterWideStringWithTheShortHeader()
    {
        // Flag 0 and length 0x7F in 7 bits make byte 0 = 0xFE; each 'x' follows as 0x78, 0x00.
        byte[] buffer = new byte[255];
        var writer = new BitWriter(buffer);

        writer.WriteWideString(new string('x', 127));

        byte[] expected = new byte[255];
        expected[0] = 0xFE;
        for (int i = 0; i < 127; i++)
            expected[1 + (2 * i)] = 0x78;
        Assert.Equal(expected, buffer);
        Assert.Equal(2040, writer.BitPosition);
    }

    [Fact]
    public void WritesA128CharacterWideStringWithTheExtendedHeader()
    {
        // Flag 1 and length 0x80 in 15 bits make the 16-bit header 0x0101; each 'x' follows as 0x78, 0x00.
        byte[] buffer = new byte[258];
        var writer = new BitWriter(buffer);

        writer.WriteWideString(new string('x', 128));

        byte[] expected = new byte[258];
        expected[0] = 0x01;
        expected[1] = 0x01;
        for (int i = 0; i < 128; i++)
            expected[2 + (2 * i)] = 0x78;
        Assert.Equal(expected, buffer);
        Assert.Equal(2064, writer.BitPosition);
    }

    [Theory]
    [InlineData(127)]
    [InlineData(128)]
    public void RoundTripsAWideStringEitherSideOfTheHeaderBoundary(int length)
    {
        // At most a 2-byte header and 2 bytes a unit.
        string value = Letters(length);
        byte[] buffer = new byte[2 + (2 * length)];
        var writer = new BitWriter(buffer);
        writer.WriteWideString(value);

        var reader = new BitReader(buffer);

        Assert.Equal(value, reader.ReadWideString());
        Assert.Equal(writer.BitPosition, reader.BitPosition);
    }

    [Fact]
    public void WritesANarrowStringAsFlagLengthThenBytes()
    {
        // Flag 0 and length 2 make byte 0 = 0b0000_0100; 'H' and 'i' follow as 0x48, 0x69.
        byte[] buffer = new byte[3];
        var writer = new BitWriter(buffer);

        writer.WriteNarrowString("Hi");

        byte[] expected = [0x04, 0x48, 0x69];
        Assert.Equal(expected, buffer);
        Assert.Equal(24, writer.BitPosition);
    }

    [Fact]
    public void ReadsANarrowStringAsFlagLengthThenBytes()
    {
        byte[] body = [0x04, 0x48, 0x69];
        var reader = new BitReader(body);

        Assert.Equal("Hi", reader.ReadNarrowString());
        Assert.Equal(24, reader.BitPosition);
    }

    [Fact]
    public void WritesANarrowUnitAboveSevenBitsAsAQuestionMark()
    {
        // U+00E9 is above 0x7F, so it writes as '?' (0x3F).
        byte[] buffer = new byte[2];
        var writer = new BitWriter(buffer);

        writer.WriteNarrowString("é");

        byte[] expected = [0x02, 0x3F];
        Assert.Equal(expected, buffer);
    }

    [Fact]
    public void ReadsANarrowByteAboveSevenBitsAsAQuestionMark()
    {
        byte[] body = [0x02, 0xE9];
        var reader = new BitReader(body);

        Assert.Equal("?", reader.ReadNarrowString());
    }

    [Fact]
    public void WritesA128CharacterNarrowStringWithTheExtendedHeader()
    {
        // Flag 1 and length 0x80 in 15 bits make the 16-bit header 0x0101; each 'x' follows as 0x78.
        byte[] buffer = new byte[130];
        var writer = new BitWriter(buffer);

        writer.WriteNarrowString(new string('x', 128));

        byte[] expected = new byte[130];
        expected[0] = 0x01;
        expected[1] = 0x01;
        for (int i = 0; i < 128; i++)
            expected[2 + i] = 0x78;
        Assert.Equal(expected, buffer);
        Assert.Equal(1040, writer.BitPosition);
    }

    [Theory]
    [InlineData(127)]
    [InlineData(128)]
    public void RoundTripsANarrowStringEitherSideOfTheHeaderBoundary(int length)
    {
        // At most a 2-byte header and a byte a unit.
        string value = Letters(length);
        byte[] buffer = new byte[2 + length];
        var writer = new BitWriter(buffer);
        writer.WriteNarrowString(value);

        var reader = new BitReader(buffer);

        Assert.Equal(value, reader.ReadNarrowString());
        Assert.Equal(writer.BitPosition, reader.BitPosition);
    }

    [Fact]
    public void WritesATerminatedWideStringAsCountUnitsThenTerminator()
    {
        // Count 3 covers 'H' (0x0048), 'i' (0x0069) and the zero terminator.
        byte[] buffer = new byte[8];
        var writer = new BitWriter(buffer);

        writer.WriteTerminatedWideString("Hi");

        byte[] expected = [0x03, 0x00, 0x48, 0x00, 0x69, 0x00, 0x00, 0x00];
        Assert.Equal(expected, buffer);
        Assert.Equal(64, writer.BitPosition);
    }

    [Fact]
    public void WritesAnEmptyTerminatedWideStringAsCountOneThenTerminator()
    {
        byte[] buffer = new byte[4];
        var writer = new BitWriter(buffer);

        writer.WriteTerminatedWideString(string.Empty);

        byte[] expected = [0x01, 0x00, 0x00, 0x00];
        Assert.Equal(expected, buffer);
        Assert.Equal(32, writer.BitPosition);
    }

    [Fact]
    public void ReadsATerminatedWideStringAndDropsTheTerminator()
    {
        byte[] body = [0x03, 0x00, 0x48, 0x00, 0x69, 0x00, 0x00, 0x00];
        var reader = new BitReader(body);

        Assert.Equal("Hi", reader.ReadTerminatedWideString());
        Assert.Equal(64, reader.BitPosition);
        Assert.Equal(0, reader.BitsRemaining);
    }

    [Fact]
    public void ReadsACountOfZeroAsAnEmptyTerminatedString()
    {
        byte[] body = [0x00, 0x00];
        var reader = new BitReader(body);

        Assert.Empty(reader.ReadTerminatedWideString());
        Assert.Equal(16, reader.BitPosition);
    }

    [Fact]
    public void ThrowsWhenAWideStringRunsPastTheEnd()
    {
        // Length 2, one unit present.
        byte[] body = [0x04, 0x41, 0x00];

        Assert.Throws<CodecException>(() => new BitReader(body).ReadWideString());
    }

    [Fact]
    public void ThrowsWhenANarrowStringRunsPastTheEnd()
    {
        // Length 2, one byte present.
        byte[] body = [0x04, 0x48];

        Assert.Throws<CodecException>(() => new BitReader(body).ReadNarrowString());
    }

    [Fact]
    public void ThrowsWhenATerminatedWideStringRunsPastTheEnd()
    {
        // Count 3, one unit present.
        byte[] body = [0x03, 0x00, 0x48, 0x00];

        Assert.Throws<CodecException>(() => new BitReader(body).ReadTerminatedWideString());
    }

    [Fact]
    public void ThrowsWhenAStringIsLongerThanTheFifteenBitLength()
    {
        // 0x8000 units is one past the 15-bit length's 0x7FFF.
        string value = new('x', 0x8000);

        Assert.Throws<CodecException>(() => new BitWriter(new byte[1]).WriteWideString(value));
        Assert.Throws<CodecException>(() => new BitWriter(new byte[1]).WriteNarrowString(value));
    }

    [Fact]
    public void ThrowsWhenATerminatedWideStringIsLongerThanTheSixteenBitCount()
    {
        // 0xFFFF units and the terminator make a count of 0x10000, one past the 16-bit count's 0xFFFF.
        string value = new('x', 0xFFFF);

        Assert.Throws<CodecException>(() => new BitWriter(new byte[1]).WriteTerminatedWideString(value));
    }

    [Theory]
    [InlineData(0, 8)]
    [InlineData(2, 40)]
    [InlineData(127, 2040)]
    [InlineData(128, 2064)]
    public void MeasureWideStringEqualsTheBitsWritten(int length, int expectedBits)
    {
        // A 1-bit flag, a 7-bit or 15-bit length, then 16 bits a unit.
        string value = Letters(length);
        var writer = new BitWriter(new byte[(expectedBits + 7) / 8]);

        writer.WriteWideString(value);

        Assert.Equal(expectedBits, BitWriter.MeasureWideString(value));
        Assert.Equal(expectedBits, writer.BitPosition);
    }

    [Theory]
    [InlineData(0, 8)]
    [InlineData(2, 24)]
    [InlineData(127, 1024)]
    [InlineData(128, 1040)]
    public void MeasureNarrowStringEqualsTheBitsWritten(int length, int expectedBits)
    {
        // A 1-bit flag, a 7-bit or 15-bit length, then 8 bits a unit.
        string value = Letters(length);
        var writer = new BitWriter(new byte[(expectedBits + 7) / 8]);

        writer.WriteNarrowString(value);

        Assert.Equal(expectedBits, BitWriter.MeasureNarrowString(value));
        Assert.Equal(expectedBits, writer.BitPosition);
    }

    [Theory]
    [InlineData(0, 32)]
    [InlineData(2, 64)]
    [InlineData(127, 2064)]
    [InlineData(128, 2080)]
    public void MeasureTerminatedWideStringEqualsTheBitsWritten(int length, int expectedBits)
    {
        // A 16-bit count, then 16 bits a unit for each character and the terminator.
        string value = Letters(length);
        var writer = new BitWriter(new byte[(expectedBits + 7) / 8]);

        writer.WriteTerminatedWideString(value);

        Assert.Equal(expectedBits, BitWriter.MeasureTerminatedWideString(value));
        Assert.Equal(expectedBits, writer.BitPosition);
    }

    private static string Letters(int length)
    {
        char[] chars = new char[length];
        for (int i = 0; i < length; i++)
            chars[i] = (char)('a' + (i % 26));
        return new string(chars);
    }
}
