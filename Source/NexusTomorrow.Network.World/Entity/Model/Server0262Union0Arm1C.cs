using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(28)]
public sealed partial class Server0262Union0Arm1C : IServer0262Union0
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
