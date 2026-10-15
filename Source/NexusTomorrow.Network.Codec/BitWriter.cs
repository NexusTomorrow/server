using System.Numerics;

namespace NexusTomorrow.Network.Codec;

/// <summary>
/// Writes a message body as the client reads it: a bit stream that fills each byte from its least significant bit, with each value's least significant bit first.
/// The caller sizes the buffer from the value's measured bits; a byte is cleared when the writer first touches it, so a rented buffer needs no clearing.
/// </summary>
public ref struct BitWriter
{
    private const int MaxShortLength = 0x7F;
    private const int MaxLongLength = 0x7FFF;

    private readonly Span<byte> _buffer;
    private int _bitPosition;

    /// <summary>
    /// Create a writer over the supplied buffer.
    /// </summary>
    public BitWriter(Span<byte> buffer)
    {
        _buffer = buffer;
        _bitPosition = 0;
    }

    /// <summary>
    /// The number of bits written so far.
    /// </summary>
    public readonly int BitPosition => _bitPosition;

    /// <summary>
    /// The number of bytes the bits written so far occupy, counting a partly written last byte.
    /// </summary>
    public readonly int BytesWritten => (_bitPosition + 7) >> 3;

    /// <summary>
    /// The bits a wide string of <paramref name="value"/> occupies.
    /// </summary>
    public static int MeasureWideString(string value)
    {
        return 1 + (value.Length > MaxShortLength ? 15 : 7) + (16 * value.Length);
    }

    /// <summary>
    /// The bits a narrow string of <paramref name="value"/> occupies.
    /// </summary>
    public static int MeasureNarrowString(string value)
    {
        return 1 + (value.Length > MaxShortLength ? 15 : 7) + (8 * value.Length);
    }

    /// <summary>
    /// The bits a terminated wide string of <paramref name="value"/> occupies.
    /// </summary>
    public static int MeasureTerminatedWideString(string value)
    {
        return 16 + (16 * (value.Length + 1));
    }

    /// <summary>
    /// Write the low <paramref name="bits"/> bits of <paramref name="value"/>, from 1 to 64 bits.
    /// </summary>
    /// <exception cref="CodecException"><paramref name="value"/> does not fit in <paramref name="bits"/> bits, or the buffer is full.</exception>
    public void WriteBits(ulong value, int bits)
    {
        if ((uint)(bits - 1) >= 64u)
            throw new ArgumentOutOfRangeException(nameof(bits), bits, "A field is 1 to 64 bits wide.");
        if (bits < 64 && (value >> bits) != 0)
            throw new CodecException($"The value {value} does not fit in {bits} bits.");
        if (_bitPosition + bits > _buffer.Length * 8)
            throw new CodecException($"A {bits}-bit write at bit {_bitPosition} runs past the end of a {_buffer.Length}-byte buffer.");

        int done = 0;
        while (done < bits)
        {
            int byteIndex = _bitPosition >> 3;
            int bitIndex = _bitPosition & 7;
            if (bitIndex == 0)
                _buffer[byteIndex] = 0;

            int take = Math.Min(8 - bitIndex, bits - done);
            int chunk = (int)((value >> done) & (ulong)((1 << take) - 1));
            _buffer[byteIndex] |= (byte)(chunk << bitIndex);
            done += take;
            _bitPosition += take;
        }
    }

    /// <summary>
    /// Write <paramref name="value"/> as a two's complement value <paramref name="bits"/> wide.
    /// </summary>
    /// <exception cref="CodecException"><paramref name="value"/> is outside the range a signed <paramref name="bits"/>-bit value holds.</exception>
    public void WriteSignedBits(long value, int bits)
    {
        if ((uint)(bits - 1) >= 64u)
            throw new ArgumentOutOfRangeException(nameof(bits), bits, "A field is 1 to 64 bits wide.");
        if (bits < 64)
        {
            long limit = 1L << (bits - 1);
            if (value < -limit || value >= limit)
                throw new CodecException($"The value {value} does not fit in a signed {bits}-bit field.");
        }

        ulong mask = bits == 64 ? ulong.MaxValue : (1UL << bits) - 1;
        WriteBits(unchecked((ulong)value) & mask, bits);
    }

    /// <summary>
    /// Write an IEEE 754 single as 32 bits.
    /// </summary>
    public void WriteFloat32(float value)
    {
        WriteBits(unchecked((uint)BitConverter.SingleToInt32Bits(value)), 32);
    }

    /// <summary>
    /// Write an IEEE 754 double as 64 bits.
    /// </summary>
    public void WriteFloat64(double value)
    {
        WriteBits(unchecked((ulong)BitConverter.DoubleToInt64Bits(value)), 64);
    }

    /// <summary>
    /// Write the client's 16-bit packed float.
    /// </summary>
    public void WritePackedFloat(float value)
    {
        WriteBits(PackedFloat.Pack(value), 16);
    }

    /// <summary>
    /// Write a vector as three IEEE 754 singles, X then Y then Z.
    /// </summary>
    public void WriteVector3(Vector3 value)
    {
        WriteFloat32(value.X);
        WriteFloat32(value.Y);
        WriteFloat32(value.Z);
    }

    /// <summary>
    /// Write a vector as three packed floats, X then Y then Z.
    /// </summary>
    public void WritePackedVector3(Vector3 value)
    {
        WritePackedFloat(value.X);
        WritePackedFloat(value.Y);
        WritePackedFloat(value.Z);
    }

    /// <summary>
    /// Write a wide string: a 1-bit flag, a length of 7 bits (flag clear) or 15 bits (flag set) in UTF-16 code units, then each code unit as 16 bits.
    /// </summary>
    public void WriteWideString(string value)
    {
        WriteStringHeader(value.Length);
        foreach (char c in value)
            WriteBits(c, 16);
    }

    /// <summary>
    /// Write a narrow string: the wide string's header with the length in bytes, then each code unit as one byte; a code unit above 0x7F writes as '?', as ASCII encoding does.
    /// </summary>
    public void WriteNarrowString(string value)
    {
        WriteStringHeader(value.Length);
        foreach (char c in value)
            WriteBits(c <= 0x7F ? c : '?', 8);
    }

    /// <summary>
    /// Write a terminated wide string: a 16-bit count of UTF-16 code units that includes the terminator, each code unit as 16 bits, then the zero terminator.
    /// </summary>
    public void WriteTerminatedWideString(string value)
    {
        if (value.Length + 1 > ushort.MaxValue)
            throw new CodecException($"A terminated string of {value.Length} characters does not fit its 16-bit count.");

        WriteBits((ulong)(value.Length + 1), 16);
        foreach (char c in value)
            WriteBits(c, 16);
        WriteBits(0, 16);
    }

    /// <summary>
    /// Write each byte of <paramref name="source"/> as 8 bits, at the current bit position.
    /// </summary>
    public void WriteBytes(ReadOnlySpan<byte> source)
    {
        foreach (byte value in source)
            WriteBits(value, 8);
    }

    /// <summary>
    /// Skip to the next byte boundary; the skipped bits are zero.
    /// </summary>
    public void AlignToByte()
    {
        _bitPosition = (_bitPosition + 7) & ~7;
    }

    private void WriteStringHeader(int length)
    {
        if (length > MaxLongLength)
            throw new CodecException($"A string of {length} units does not fit its 15-bit length.");

        bool extended = length > MaxShortLength;
        WriteBits(extended ? 1UL : 0UL, 1);
        WriteBits((ulong)length, extended ? 15 : 7);
    }
}
