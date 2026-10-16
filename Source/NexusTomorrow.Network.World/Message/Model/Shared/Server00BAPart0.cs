using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server00BAPart0
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(18)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(64)] public ulong Unknown5 { get; set; }
    [UInt(64)] public ulong Unknown6 { get; set; }
    [UInt(18)] public uint Unknown7 { get; set; }
    [FixedArray(20), UInt(8)] public byte[] Unknown8 { get; set; } = new byte[20];
    [FixedArray(32), UInt(8)] public byte[] Unknown9 { get; set; } = new byte[32];
    [FixedArray(2), UInt(64)] public ulong[] Unknown10 { get; set; } = new ulong[2];
    [FixedArray(2), UInt(4)] public byte[] Unknown11 { get; set; } = new byte[2];
    [UInt(32)] public uint Unknown12 { get; set; }
}
