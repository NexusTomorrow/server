using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(17)]
public sealed partial class Server0262Union2Arm11 : IServer0262Union2
{
    /// <summary>
    /// The splines the unit faces along, chained into one path, by id. The client looks each up as the command starts.
    /// </summary>
    [VerifiedMember, Array(CountBits = 10), UInt(32)] public List<uint> SplineIds { get; set; } = [];

    [PackedFloat] public float Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [Float32] public float Unknown4 { get; set; }

    [UInt(4)] public byte Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, the client blends away the jump to the segment; when clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
