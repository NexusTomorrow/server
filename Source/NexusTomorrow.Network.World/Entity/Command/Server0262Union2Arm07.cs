using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(7)]
public sealed partial class Server0262Union2Arm07 : IServer0262Union2
{
    [Float32] public Vector3 Unknown0 { get; set; }
    [Float32] public Vector3 Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
}
