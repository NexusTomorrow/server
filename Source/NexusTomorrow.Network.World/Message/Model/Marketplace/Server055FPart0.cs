using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireStruct]
public sealed partial class Server055FPart0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
    [UInt(64)] public ulong Unknown4 { get; set; }
    [UInt(64)] public ulong Unknown5 { get; set; }
    [UInt(64)] public ulong Unknown6 { get; set; }
    [UInt(18)] public uint Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [UInt(18)] public uint Unknown9 { get; set; }
    [Array(CountBits = 3), UInt(32)] public List<uint> Unknown10 { get; set; } = [];
    [UInt(64)] public ulong Unknown11 { get; set; }
    [UInt(32)] public uint Unknown12 { get; set; }
    [UInt(64)] public ulong Unknown13 { get; set; }
    [UInt(32)] public uint Unknown14 { get; set; }
}
