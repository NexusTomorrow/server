using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(35)]
public sealed partial class Server0262Union0Arm23 : IServer0262Union0
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
}
