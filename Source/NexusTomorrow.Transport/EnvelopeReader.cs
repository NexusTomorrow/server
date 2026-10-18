using System.Buffers.Binary;

namespace NexusTomorrow.Transport;

/// <summary>
/// Opens a blob that holds one inner frame: the client's envelope (encrypted after the length) and its nested wrapper (in clear).
/// </summary>
public static class EnvelopeReader
{
    /// <summary>
    /// Decrypt <paramref name="blob"/> in place when <paramref name="cipher"/> is given, and return the inner frame's opcode and body.
    /// False when the blob's leading length is not its own length or is shorter than an inner header; nothing is decrypted then.
    /// </summary>
    public static bool TryOpen(Span<byte> blob, PacketCipher? cipher, out ushort opcode, out Span<byte> body)
    {
        opcode = 0;
        body = default;
        if (blob.Length < FrameReader.HeaderBytes)
            return false;
        if (BinaryPrimitives.ReadUInt32LittleEndian(blob) != (uint)blob.Length)
            return false;

        cipher?.Decrypt(blob[4..]);
        opcode = BinaryPrimitives.ReadUInt16LittleEndian(blob[4..]);
        body = blob[FrameReader.HeaderBytes..];
        return true;
    }
}
