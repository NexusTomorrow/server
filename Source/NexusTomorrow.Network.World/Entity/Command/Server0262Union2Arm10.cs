using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(16)]
public sealed partial class Server0262Union2Arm10 : IServer0262Union2
{
    /// <summary>
    /// The spline the unit faces along, by id, which the client looks up as the command starts.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint SplineId { get; set; }

    [PackedFloat] public float Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [UInt(4)] public byte Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, the client blends away the jump to the segment; when clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown6 { get; set; }
}
