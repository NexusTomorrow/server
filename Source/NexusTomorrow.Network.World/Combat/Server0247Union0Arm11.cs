using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(17)]
public sealed partial class Server0247Union0Arm11 : IServer0247Union0
{
    [UInt(1)] public bool Unknown0 { get; set; }
    public Server0247Part0 Unknown1 { get; set; } = new();
}
