using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(1)]
public sealed partial class Server0247Union0Arm01 : IServer0247Union0
{
    [UInt(5)] public byte Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(4)] public byte Unknown3 { get; set; }
    [UInt(14)] public ushort Unknown4 { get; set; }
    public Server0247Part0 Unknown5 { get; set; } = new();
}
