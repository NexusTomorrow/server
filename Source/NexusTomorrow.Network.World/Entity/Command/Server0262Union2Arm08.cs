using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(8)]
public sealed partial class Server0262Union2Arm08 : IServer0262Union2
{
    [PackedFloat] public Vector3 Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
