using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(26)]
public sealed partial class Server0247Union0Arm1A : IServer0247Union0
{

    [UInt(1)] public bool Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    public Server0247Part0 Unknown2 { get; set; } = new();
}
