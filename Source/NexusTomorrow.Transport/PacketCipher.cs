using System.Buffers.Binary;

namespace NexusTomorrow.Transport;

/// <summary>
/// The cipher the client applies to every enveloped message. A key gives a 128-byte table and an 8-byte seed; each message is
/// enciphered on its own from that seed, starting at a table block chosen by the message's length, so one instance serves both
/// directions of a connection and holds no state between messages.
/// </summary>
public sealed class PacketCipher
{
    private const ulong Multiplier = 0xAA7F8EA9;
    private const uint LengthMultiplier = 0xAA7F8EAA;
    private const ulong InitialValue = 0x718DA9074F2DEB91;
    private const ulong BuildSalt = 0x5B88D61139619662;
    private const ulong Build = 16042;
    private const ulong ProtocolVersion = 0x097998A0;
    private const int TableBytes = 128;
    private const int SessionKeyBytes = 16;

    private readonly byte[] _table = new byte[TableBytes];
    private readonly ulong _seed;

    /// <summary>
    /// Create the cipher for <paramref name="key"/>.
    /// </summary>
    public PacketCipher(ulong key)
    {
        unchecked
        {
            ulong value = InitialValue;
            ulong word = (value + key) * Multiplier;
            for (int offset = 0; offset < TableBytes; offset += 8)
            {
                BinaryPrimitives.WriteUInt64LittleEndian(_table.AsSpan(offset), word);
                value = (value + word) * Multiplier;
                word = (key + word) * Multiplier;
            }

            _seed = value;
        }
    }

    /// <summary>
    /// The key every connection starts under, fixed by the client's build (16042) and protocol version (0x097998A0).
    /// </summary>
    public static ulong BuildKey { get; } = ComputeBuildKey();

    /// <summary>
    /// The key a world connection switches to after the client's greeting: the 16 session key bytes, in wire order, folded and then
    /// combined with <see cref="BuildKey"/>.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="sessionKey"/> is not 16 bytes.</exception>
    public static ulong SessionKey(ReadOnlySpan<byte> sessionKey)
    {
        if (sessionKey.Length != SessionKeyBytes)
            throw new ArgumentException($"A session key is {SessionKeyBytes} bytes.", nameof(sessionKey));

        unchecked
        {
            ulong key = InitialValue;
            foreach (byte value in sessionKey)
                key = (key + value) * Multiplier;
            return (key + BuildKey) * Multiplier;
        }
    }

    /// <summary>
    /// Encrypt <paramref name="data"/> in place: the bytes of one message after its length.
    /// </summary>
    public void Encrypt(Span<byte> data)
    {
        Span<byte> state = stackalloc byte[8];
        BinaryPrimitives.WriteUInt64LittleEndian(state, _seed);
        uint block = unchecked(LengthMultiplier * (uint)data.Length);
        int offset = 0;
        for (int i = 0; i < data.Length; i++)
        {
            int lane = i & 7;
            if (lane == 0)
                offset = (int)(unchecked(block++) & 0xF) * 8;

            byte cipher = (byte)(state[lane] ^ data[i] ^ _table[offset + lane]);
            data[i] = cipher;
            state[lane] = cipher;
        }
    }

    /// <summary>
    /// Decrypt <paramref name="data"/> in place: the bytes of one message after its length.
    /// </summary>
    public void Decrypt(Span<byte> data)
    {
        Span<byte> state = stackalloc byte[8];
        BinaryPrimitives.WriteUInt64LittleEndian(state, _seed);
        uint block = unchecked(LengthMultiplier * (uint)data.Length);
        int offset = 0;
        for (int i = 0; i < data.Length; i++)
        {
            int lane = i & 7;
            if (lane == 0)
                offset = (int)(unchecked(block++) & 0xF) * 8;

            byte cipher = data[i];
            data[i] = (byte)(state[lane] ^ cipher ^ _table[offset + lane]);
            state[lane] = cipher;
        }
    }

    private static ulong ComputeBuildKey()
    {
        unchecked
        {
            ulong key = (InitialValue + BuildSalt) * Multiplier;
            key = (key + Build) * Multiplier;
            return (key + ProtocolVersion) * Multiplier;
        }
    }
}
