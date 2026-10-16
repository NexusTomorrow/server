using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(2)]
public sealed partial class Server0262Union2Arm02 : IServer0262Union2
{
    [Float32] public Vector3 Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
