using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(10)]
public sealed partial class Server0262Union2Arm0A : IServer0262Union2
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
