using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(16)]
public sealed partial class Server0247Union0Arm10 : IServer0247Union0
{

    [Float32] public float Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
    public Server0247Part0 Unknown3 { get; set; } = new();
}
