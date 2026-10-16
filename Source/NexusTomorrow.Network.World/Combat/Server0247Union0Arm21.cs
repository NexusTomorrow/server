using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(33)]
public sealed partial class Server0247Union0Arm21 : IServer0247Union0
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
}
