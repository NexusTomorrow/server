using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(23)]
public sealed partial class Server0262Union0Arm17 : IServer0262Union0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
    [UInt(5)] public byte Unknown3 { get; set; }
    [UInt(5)] public byte Unknown4 { get; set; }
    [UInt(2)] public byte Unknown5 { get; set; }
    [UInt(64)] public ulong Unknown6 { get; set; }
    [WideString] public string Unknown7 { get; set; } = "";
    [UInt(4)] public byte Unknown8 { get; set; }
    [Array(CountBits = 5), UInt(64)] public List<ulong> Unknown9 { get; set; } = [];
    [Array(CountBits = 6), Float32] public List<float> Unknown10 { get; set; } = [];
    [UInt(14)] public ushort Unknown11 { get; set; }
}
