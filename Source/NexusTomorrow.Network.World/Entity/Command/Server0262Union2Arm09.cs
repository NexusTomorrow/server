using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(9)]
public sealed partial class Server0262Union2Arm09 : IServer0262Union2
{
    [UInt(10), Count] public ushort Unknown0 { get; set; }
    [Array(CountFrom = nameof(Unknown0)), UInt(32)] public List<uint> Unknown1 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown0)), PackedFloat] public List<Vector3> Unknown2 { get; set; } = [];
    [UInt(2)] public byte Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
}
