using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(22)]
public sealed partial class Server0262Union2Arm16 : IServer0262Union2
{
    [PackedFloat] public float Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
