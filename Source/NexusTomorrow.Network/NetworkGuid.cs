using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network;

// UUID structure in Windows
[WireStruct]
public sealed partial class NetworkGuid
{
    [SInt(32)] public int Data1 { get; private set; }
    [SInt(16)] public short Data2 { get; private set; }
    [SInt(16)] public short Data3 { get; private set; }
    [FixedArray(8), UInt(8)] public byte[] Data4 { get; private set; } = new byte[8];
}
