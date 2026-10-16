using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(8)]
public sealed partial class Client01C3Union0Arm08 : IClient01C3Union0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(18)] public uint Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(64)] public ulong Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(8)] public byte Unknown8 { get; set; }
    [UInt(18)] public uint Unknown9 { get; set; }
    [Array(CountBits = 3), Float32] public List<float> Unknown10 { get; set; } = [];
    [Array(CountBits = 4), UInt(32)] public List<uint> Unknown11 { get; set; } = [];
}
