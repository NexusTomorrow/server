using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireStruct]
public partial class Server0501Part0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [UInt(14)] public ushort Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
    [WideString] public string Unknown4 { get; set; } = "";
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
}
