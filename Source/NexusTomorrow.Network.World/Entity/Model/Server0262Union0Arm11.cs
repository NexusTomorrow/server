using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(17)]
public sealed partial class Server0262Union0Arm11 : IServer0262Union0
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
    [UInt(14)] public ushort Unknown2 { get; set; }
}
