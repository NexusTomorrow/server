using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(7)]
public sealed partial class Server0262Union2Arm07 : IServer0262Union2
{
    /// <summary>
    /// The position the unit's arc reaches at its end.
    /// </summary>
    [VerifiedMember, Float32] public Vector3 TargetPosition { get; set; }

    [Float32] public Vector3 Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, on a unit the client does not move, the client blends away the jump to the segment; when
    /// clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
