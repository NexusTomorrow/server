using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(5)]
public sealed partial class Server0262Union2Arm05 : IServer0262Union2
{
    /// <summary>
    /// The spline the unit's position follows, by id, which the client looks up as the command starts.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint SplineId { get; set; }

    [Float32] public float Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [PackedFloat] public Vector3 Unknown3 { get; set; }
    [UInt(4)] public byte Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, on a unit the client does not move, the client blends away the jump to the segment; when
    /// clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown7 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown8 { get; set; }
}
