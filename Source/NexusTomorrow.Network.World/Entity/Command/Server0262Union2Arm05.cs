using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Spline;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(5)]
public sealed partial class Server0262Union2Arm05 : IServer0262Union2
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [PackedFloat] public Vector3 Unknown3 { get; set; }
    [UInt(4)] public byte Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
    [UInt(1)] public bool Unknown7 { get; set; }
    [UInt(1)] public bool Unknown8 { get; set; }
}
