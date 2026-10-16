using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(17)]
public sealed partial class Server0262Union2Arm11 : IServer0262Union2
{
    [Array(CountBits = 10), UInt(32)] public List<uint> Unknown0 { get; set; } = [];

    [PackedFloat] public float Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [Float32] public float Unknown4 { get; set; }

    [UInt(4)] public byte Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(1)] public bool Unknown8 { get; set; }
}
