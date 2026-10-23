using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 2: the unit's position.
/// </summary>
[WireStruct]
[UnionCase(2)]
public sealed partial class Server0262Union2Arm02 : IServer0262Union2
{
    /// <summary>
    /// The position, in world coordinates while command 1 is 0.
    /// </summary>
    [VerifiedMember, Float32] public Vector3 Unknown0 { get; set; }

    /// <summary>
    /// When set, and the unit's position follows a keyed track, this position becomes the track's next key; when clear, it starts a new
    /// track.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown1 { get; set; }
}
