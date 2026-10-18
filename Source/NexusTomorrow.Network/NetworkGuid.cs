using System.Buffers.Binary;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network;

/// <summary>
/// A GUID in the Windows layout, in wire order: a 32-bit part, two 16-bit parts and eight bytes.
/// </summary>
[WireStruct]
public sealed partial class NetworkGuid
{
    /// <summary>
    /// The first 32 bits.
    /// </summary>
    [SInt(32)] public int Data1 { get; set; }

    /// <summary>
    /// The next 16 bits.
    /// </summary>
    [SInt(16)] public short Data2 { get; set; }

    /// <summary>
    /// The 16 bits after those.
    /// </summary>
    [SInt(16)] public short Data3 { get; set; }

    /// <summary>
    /// The last eight bytes, in order.
    /// </summary>
    [FixedArray(8), UInt(8)] public byte[] Data4 { get; set; } = new byte[8];

    /// <summary>
    /// Create a value from 16 bytes in wire order, the layout <see cref="Guid.TryWriteBytes(Span{byte})"/> writes: three little-endian
    /// fields, then eight bytes in order.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="bytes"/> is not 16 bytes.</exception>
    public static NetworkGuid FromBytes(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length != 16)
            throw new ArgumentException("A GUID is 16 bytes.", nameof(bytes));

        return new NetworkGuid
        {
            Data1 = BinaryPrimitives.ReadInt32LittleEndian(bytes),
            Data2 = BinaryPrimitives.ReadInt16LittleEndian(bytes[4..]),
            Data3 = BinaryPrimitives.ReadInt16LittleEndian(bytes[6..]),
            Data4 = bytes[8..16].ToArray(),
        };
    }

    /// <summary>
    /// Create a value from <paramref name="value"/>.
    /// </summary>
    public static NetworkGuid FromGuid(Guid value)
    {
        Span<byte> bytes = stackalloc byte[16];
        value.TryWriteBytes(bytes);
        return FromBytes(bytes);
    }

    /// <summary>
    /// Write the 16 bytes in wire order to <paramref name="destination"/>.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="destination"/> is shorter than 16 bytes.</exception>
    public void CopyTo(Span<byte> destination)
    {
        if (destination.Length < 16)
            throw new ArgumentException("A GUID needs 16 bytes.", nameof(destination));

        BinaryPrimitives.WriteInt32LittleEndian(destination, Data1);
        BinaryPrimitives.WriteInt16LittleEndian(destination[4..], Data2);
        BinaryPrimitives.WriteInt16LittleEndian(destination[6..], Data3);
        Data4.AsSpan(0, 8).CopyTo(destination[8..]);
    }

    /// <summary>
    /// The value as a <see cref="Guid"/>.
    /// </summary>
    public Guid ToGuid()
    {
        Span<byte> bytes = stackalloc byte[16];
        CopyTo(bytes);
        return new Guid(bytes);
    }
}
