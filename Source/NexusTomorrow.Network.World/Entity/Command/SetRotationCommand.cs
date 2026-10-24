using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 14: the unit's facing.
/// </summary>
[WireStruct]
[UnionCase(14)]
public sealed partial class SetRotationCommand : IServer0262Union2
{
    /// <summary>
    /// The facing, as three values. While <see cref="SetPlatformCommand.PlatformUnitId"/> is 0, the facing the client sends for the unit it
    /// moves, as it samples it each frame, is its yaw first, in radians, the angle atan2(-x, -z) of the direction it faces, and 0 for the
    /// other two.
    /// </summary>
    [VerifiedMember, Float32] public Vector3 Rotation { get; set; }

    /// <summary>
    /// Whether the value blends into the unit's motion. When set and the newest segment on this command's channel is a keyed one that this
    /// command made, the value becomes that segment's next key; otherwise the value starts a new segment, and on a unit the client does not
    /// move the client blends away the jump to it. When clear, the value starts a new segment and the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
