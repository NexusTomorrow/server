using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(11)]
public sealed partial class Server0262Union2Arm0B : IServer0262Union2
{
    [PackedFloat] public Vector3 Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
