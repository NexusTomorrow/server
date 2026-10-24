using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(19)]
public sealed partial class Server0262Union2Arm13 : IServer0262Union2
{
    /// <summary>
    /// The point the unit turns to face.
    /// </summary>
    [VerifiedMember, Float32] public Vector3 Position { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, the client blends away the jump to the segment; when clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
