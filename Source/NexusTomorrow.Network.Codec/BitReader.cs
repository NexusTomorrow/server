using System.Numerics;

namespace NexusTomorrow.Network.Codec;

/// <summary>
/// Reads a message body as the client writes it: a bit stream that fills each byte from its least significant bit, with each value's least significant bit first.
/// </summary>
public ref struct BitReader
{
    private const int StackStringLimit = 256;

    private readonly ReadOnlySpan<byte> _buffer;
    private int _bitPosition;

    /// <summary>
    /// Create a reader over the supplied message body.
    /// </summary>
    public BitReader(ReadOnlySpan<byte> buffer)
    {
        _buffer = buffer;
        _bitPosition = 0;
    }

    /// <summary>
    /// The number of bits read so far.
    /// </summary>
    public readonly int BitPosition => _bitPosition;

    /// <summary>
    /// The number of bits left in the body.
    /// </summary>
    public readonly int BitsRemaining => (_buffer.Length * 8) - _bitPosition;

    /// <summary>
    /// Read an unsigned value <paramref name="bits"/> wide, from 1 to 64 bits.
    /// </summary>
    /// <exception cref="CodecException">The body has fewer than <paramref name="bits"/> bits left.</exception>
    public ulong ReadBits(int bits)
    {
        if ((uint)(bits - 1) >= 64u)
            throw new ArgumentOutOfRangeException(nameof(bits), bits, "A field is 1 to 64 bits wide.");
        if (bits > BitsRemaining)
            throw new CodecException($"A {bits}-bit read at bit {_bitPosition} runs past the end of a {_buffer.Length}-byte body.");

        ulong value = 0;
        int done = 0;
        while (done < bits)
        {
            int byteIndex = _bitPosition >> 3;
            int bitIndex = _bitPosition & 7;
            int take = Math.Min(8 - bitIndex, bits - done);
            ulong chunk = (ulong)((_buffer[byteIndex] >> bitIndex) & ((1 << take) - 1));
            value |= chunk << done;
            done += take;
            _bitPosition += take;
        }

        return value;
    }

    /// <summary>
    /// Read a two's complement value <paramref name="bits"/> wide and sign-extend it.
    /// </summary>
    public long ReadSignedBits(int bits)
    {
        ulong raw = ReadBits(bits);
        int shift = 64 - bits;
        return (long)(raw << shift) >> shift;
    }

    /// <summary>
    /// Read an IEEE 754 single from 32 bits.
    /// </summary>
    public float ReadFloat32()
    {
        return BitConverter.Int32BitsToSingle(unchecked((int)(uint)ReadBits(32)));
    }

    /// <summary>
    /// Read an IEEE 754 double from 64 bits.
    /// </summary>
    public double ReadFloat64()
    {
        return BitConverter.Int64BitsToDouble(unchecked((long)ReadBits(64)));
    }

    /// <summary>
    /// Read the client's 16-bit packed float.
    /// </summary>
    public float ReadPackedFloat()
    {
        return PackedFloat.Unpack((ushort)ReadBits(16));
    }

    /// <summary>
    /// Read three IEEE 754 singles as a vector, X then Y then Z.
    /// </summary>
    public Vector3 ReadVector3()
    {
        float x = ReadFloat32();
        float y = ReadFloat32();
        float z = ReadFloat32();
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Read three packed floats as a vector, X then Y then Z.
    /// </summary>
    public Vector3 ReadPackedVector3()
    {
        float x = ReadPackedFloat();
        float y = ReadPackedFloat();
        float z = ReadPackedFloat();
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Read a wide string: a 1-bit flag, a length of 7 bits (flag clear) or 15 bits (flag set) in UTF-16 code units, then each code unit as 16 bits.
    /// </summary>
    public string ReadWideString()
    {
        bool extended = ReadBits(1) != 0;
        int length = (int)ReadBits(extended ? 15 : 7);
        if (length == 0)
            return string.Empty;
        if (length * 16 > BitsRemaining)
            throw new CodecException($"A {length}-character wide string at bit {_bitPosition} runs past the end of the body.");

        Span<char> chars = length <= StackStringLimit ? stackalloc char[length] : new char[length];
        for (int i = 0; i < length; i++)
            chars[i] = (char)ReadBits(16);
        return new string(chars);
    }

    /// <summary>
    /// Read a narrow string: the wide string's header with the length in bytes, then each byte as 8 bits; a byte above 0x7F reads as '?', as ASCII decoding does.
    /// </summary>
    public string ReadNarrowString()
    {
        bool extended = ReadBits(1) != 0;
        int length = (int)ReadBits(extended ? 15 : 7);
        if (length == 0)
            return string.Empty;
        if (length * 8 > BitsRemaining)
            throw new CodecException($"A {length}-byte narrow string at bit {_bitPosition} runs past the end of the body.");

        Span<char> chars = length <= StackStringLimit ? stackalloc char[length] : new char[length];
        for (int i = 0; i < length; i++)
        {
            ulong value = ReadBits(8);
            chars[i] = value <= 0x7F ? (char)value : '?';
        }

        return new string(chars);
    }

    /// <summary>
    /// Read a terminated wide string: a 16-bit count of UTF-16 code units that includes the terminator, then each code unit as 16 bits; the terminator is dropped.
    /// </summary>
    public string ReadTerminatedWideString()
    {
        int count = (int)ReadBits(16);
        if (count == 0)
            return string.Empty;
        if (count * 16 > BitsRemaining)
            throw new CodecException($"A {count}-unit terminated string at bit {_bitPosition} runs past the end of the body.");

        int length = count - 1;
        Span<char> chars = length <= StackStringLimit ? stackalloc char[length] : new char[length];
        for (int i = 0; i < length; i++)
            chars[i] = (char)ReadBits(16);
        ReadBits(16);
        return new string(chars);
    }

    /// <summary>
    /// Fill <paramref name="destination"/> with bytes read as 8 bits each, at the current bit position.
    /// </summary>
    public void ReadBytes(Span<byte> destination)
    {
        if (destination.Length * 8 > BitsRemaining)
            throw new CodecException($"A {destination.Length}-byte read at bit {_bitPosition} runs past the end of the body.");

        for (int i = 0; i < destination.Length; i++)
            destination[i] = (byte)ReadBits(8);
    }

    /// <summary>
    /// Skip to the next byte boundary; a reader already on one stays where it is.
    /// </summary>
    public void AlignToByte()
    {
        _bitPosition = (_bitPosition + 7) & ~7;
    }
}
