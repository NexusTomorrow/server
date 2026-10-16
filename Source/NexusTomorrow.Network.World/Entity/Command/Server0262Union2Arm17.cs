using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(23)]
public sealed partial class Server0262Union2Arm17 : IServer0262Union2
{
    [UInt(8), Count] public byte Unknown0 { get; set; }
    [Array(CountFrom = nameof(Unknown0)), UInt(32)] public List<uint> Unknown1 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown0)), PackedFloat] public List<float> Unknown2 { get; set; } = [];
    [UInt(2)] public byte Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
}
