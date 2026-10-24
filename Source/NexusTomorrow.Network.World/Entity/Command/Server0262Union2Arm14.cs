using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(20)]
public sealed partial class Server0262Union2Arm14 : IServer0262Union2
{
    [Float32] public Vector3 Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [PackedFloat] public float Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, the client blends away the jump to the segment; when clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
