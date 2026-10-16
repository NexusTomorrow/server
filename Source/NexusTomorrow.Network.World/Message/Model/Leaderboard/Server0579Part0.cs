using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireStruct]
public sealed partial class Server0579Part0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [WideString] public string Unknown8 { get; set; } = "";
    [Array(CountBits = 32)] public List<Server0579Part1> Unknown9 { get; set; } = [];
}
