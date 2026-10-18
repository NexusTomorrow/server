using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server07F2Part0
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
    [UInt(1)] public bool Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [UInt(64)] public ulong Unknown9 { get; set; }
    [UInt(32)] public uint Unknown10 { get; set; }
    [UInt(32)] public uint Unknown11 { get; set; }
    [Array(CountBits = 32)] public List<Client00B5Part0> Unknown12 { get; set; } = [];
}
