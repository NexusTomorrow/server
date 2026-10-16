using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(30)]
public sealed partial class Server0247Union0Arm1E : IServer0247Union0
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(18)] public uint Unknown1 { get; set; }
}
