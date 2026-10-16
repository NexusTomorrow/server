using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(24)]
public sealed partial class Server0247Union0Arm18 : IServer0247Union0
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
