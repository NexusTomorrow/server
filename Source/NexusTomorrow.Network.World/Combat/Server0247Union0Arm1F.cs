using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(31)]
public sealed partial class Server0247Union0Arm1F : IServer0247Union0
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
