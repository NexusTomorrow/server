using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(29)]
public sealed partial class Server0262Union2Arm1D : IServer0262Union2
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
