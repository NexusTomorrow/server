using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(9)]
public sealed partial class Server0262Union0Arm09 : IServer0262Union0
{
    [WireStruct]
    public sealed partial class Server0262Part1
    {
        [UInt(2)] public byte Unknown0 { get; set; }
        [UInt(3)] public byte Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
    }

    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [Array(CountBits = 3)] public List<Server0262Part1> Unknown3 { get; set; } = [];
}
