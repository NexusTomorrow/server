using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(22)]
public sealed partial class Server0247Union0Arm16 : IServer0247Union0
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
