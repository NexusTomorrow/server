using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(12)]
public sealed partial class Server0247Union0Arm0C : IServer0247Union0
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(8)] public byte Unknown3 { get; set; }
    public Server0247Part0 Unknown4 { get; set; } = new();
}
