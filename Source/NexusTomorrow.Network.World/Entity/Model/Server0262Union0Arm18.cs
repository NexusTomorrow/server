using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(24)]
public sealed partial class Server0262Union0Arm18 : IServer0262Union0
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(15)] public ushort Unknown2 { get; set; }
    [WideString] public string Unknown3 { get; set; } = "";
}
