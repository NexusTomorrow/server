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
}
