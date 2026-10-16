using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Spline;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(4)]
public sealed partial class Server0262Union2Arm04 : IServer0262Union2
{
    [Array(CountBits = 10), Float32] public List<Vector3> Unknown0 { get; set; } = [];
    [PackedFloat] public float Unknown1 { get; set; }
    [UInt(2)] public byte Unknown2 { get; set; }
    [UInt(4)] public byte Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
}
