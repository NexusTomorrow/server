using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(26)]
public sealed partial class Server0262Union0Arm1A : IServer0262Union0
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
