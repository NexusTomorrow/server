using System.Numerics;

namespace NexusTomorrow.Network.Codec.Tests;

// Holds the codec to the legacy GamePacketReader and GamePacketWriter it replaces, over one scripted message;
// this class is deleted with those two types. The legacy writer silently truncates a value wider than its field,
// so every scripted value fits its width.
public sealed class LegacyParityTests
{
    private const byte ThreeBitValue = 0b101;
    private const ushort FourteenBitValue = 0x2ABC;
    private const uint ThirtyTwoBitValue = 0xDEADBEEF;
    private const ulong SixtyFourBitValue = 0x0123456789ABCDEF;
    private const float SingleValue = 3.25f;
    private const float PackedValue = 0.1f;
    private const string NarrowText = "naïve";
    private const string TerminatedText = "Hi!";

    // 1 + 3 + 14 + 32 + 64 + 32 + 16 + 96 bits of fields, the wide string's 16-bit header and 130 units (2096), the
    // narrow string's 8-bit header and 5 bytes (48), the terminated string's count and 4 units (80), and 4 raw bytes
    // (32): 2514 bits, which end 2 bits into byte 315.
    private const int ScriptBits = 2514;
    private const int ScriptBytes = 315;

    private static readonly Vector3 s_vector = new(1.5f, -2.25f, 1024.0f);
    private static readonly string s_wideText = WideText();
    private static readonly byte[] s_rawBytes = [0x00, 0x7F, 0x80, 0xFF];

    [Fact]
    public void WriterProducesTheLegacyWriterBytes()
    {
        byte[] legacy = LegacyBytes();
        byte[] buffer = new byte[ScriptBytes];
        var writer = new BitWriter(buffer);

        writer.WriteBits(1, 1);
        writer.WriteBits(ThreeBitValue, 3);
        writer.WriteBits(FourteenBitValue, 14);
        writer.WriteBits(ThirtyTwoBitValue, 32);
        writer.WriteBits(SixtyFourBitValue, 64);
        writer.WriteFloat32(SingleValue);
        writer.WritePackedFloat(PackedValue);
        writer.WriteVector3(s_vector);
        writer.WriteWideString(s_wideText);
        writer.WriteNarrowString(NarrowText);
        writer.WriteTerminatedWideString(TerminatedText);
        writer.WriteBytes(s_rawBytes);

        Assert.Equal(ScriptBits, writer.BitPosition);
        Assert.Equal(ScriptBytes, writer.BytesWritten);
        Assert.Equal(legacy, buffer);
    }

    [Fact]
    public void ReaderReadsWhatTheLegacyReaderReads()
    {
        byte[] body = LegacyBytes();
        using var stream = new MemoryStream(body);
        using var legacy = new GamePacketReader(stream);
        var reader = new BitReader(body);
        byte[] rawBytes = new byte[s_rawBytes.Length];

        Assert.Equal(legacy.ReadBit(), reader.ReadBits(1) != 0);
        Assert.Equal(legacy.ReadByte(3), (byte)reader.ReadBits(3));
        Assert.Equal(legacy.ReadUShort(14), (ushort)reader.ReadBits(14));
        Assert.Equal(legacy.ReadUInt(), (uint)reader.ReadBits(32));
        Assert.Equal(legacy.ReadULong(), reader.ReadBits(64));
        Assert.Equal(legacy.ReadSingle(), reader.ReadFloat32());
        Assert.Equal(legacy.ReadPackedFloat(), reader.ReadPackedFloat());
        Assert.Equal(legacy.ReadVector3(), reader.ReadVector3());
        Assert.Equal(legacy.ReadWideString(), reader.ReadWideString());
        Assert.Equal(legacy.ReadString(), reader.ReadNarrowString());
        Assert.Equal(legacy.ReadWideStringFixed(), reader.ReadTerminatedWideString());
        reader.ReadBytes(rawBytes);
        Assert.Equal(legacy.ReadBytes((uint)s_rawBytes.Length), rawBytes);
        Assert.Equal(ScriptBits, reader.BitPosition);
    }

    private static byte[] LegacyBytes()
    {
        using var stream = new MemoryStream();
        using var legacy = new GamePacketWriter(stream);

        legacy.Write(true);
        legacy.Write(ThreeBitValue, 3);
        legacy.Write(FourteenBitValue, 14);
        legacy.Write(ThirtyTwoBitValue);
        legacy.Write(SixtyFourBitValue);
        legacy.Write(SingleValue);
        legacy.WritePackedFloat(PackedValue);
        legacy.WriteVector3(s_vector);
        legacy.WriteStringWide(s_wideText);
        legacy.WriteString(NarrowText);
        legacy.WriteStringFixed(TerminatedText);
        legacy.WriteBytes(s_rawBytes);
        legacy.FlushBits();

        return stream.ToArray();
    }

    private static string WideText()
    {
        // 130 units from U+0400 up: past the 7-bit length, every unit with a nonzero high byte and none a surrogate,
        // which the legacy writer's UTF-16 encoder would replace.
        char[] chars = new char[130];
        for (int i = 0; i < chars.Length; i++)
            chars[i] = (char)(0x0400 + i);
        return new string(chars);
    }
}
