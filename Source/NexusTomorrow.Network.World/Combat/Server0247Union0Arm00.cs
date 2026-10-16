using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(0)]
public sealed partial class Server0247Union0Arm00 : IServer0247Union0
{

    [UInt(32)] public uint Unknown0 { get; set; }
    public Server0247Part0 Unknown1 { get; set; } = new();
}
