using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(6)]
public sealed partial class Server0262Union2Arm06 : IServer0262Union2
{
    [Array(CountBits = 10), UInt(32)] public List<uint> Unknown0 { get; set; } = [];
    [PackedFloat] public float Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [Float32] public float Unknown4 { get; set; }
    [PackedFloat] public Vector3 Unknown5 { get; set; }
    [UInt(4)] public byte Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [UInt(1)] public bool Unknown9 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(1)] public bool Unknown10 { get; set; }
}
