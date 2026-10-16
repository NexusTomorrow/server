using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class Server0493Part5
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
    [UInt(2)] public byte Unknown3 { get; set; }
    [UInt(8)] public byte Unknown4 { get; set; }
    [UInt(24)] public uint Unknown5 { get; set; }
    [UInt(8)] public byte Unknown6 { get; set; }
    [UInt(24)] public uint Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [Float32] public float Unknown9 { get; set; }
    [UInt(32)] public uint Unknown10 { get; set; }
    [UInt(32)] public uint Unknown11 { get; set; }
    [UInt(32)] public uint Unknown12 { get; set; }
    [WideString] public string Unknown13 { get; set; } = "";
    [UInt(32)] public uint Unknown14 { get; set; }
    [UInt(32)] public uint Unknown15 { get; set; }
}
