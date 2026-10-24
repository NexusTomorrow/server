using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 8: the unit's velocity.
/// </summary>
[WireStruct]
[UnionCase(8)]
public sealed partial class SetVelocityCommand : IServer0262Union2
{
    /// <summary>
    /// The unit's velocity, which follows <see cref="SetMoveCommand.CommandedVelocity"/>. A position segment that starts takes it as the
    /// unit's shown velocity. In the state the client derives from a unit's channels, bit 0x1 of <see cref="SetStateCommand.State"/> is
    /// set while this is zero.
    /// </summary>
    [VerifiedMember, PackedFloat] public Vector3 Velocity { get; set; }

    /// <summary>
    /// Whether the value blends into the unit's motion. When set and the newest segment on this command's channel is a keyed one that this
    /// command made, the value becomes that segment's next key; otherwise the value starts a new segment, and on a unit the client does not
    /// move the client blends away the jump to it. When clear, the value starts a new segment and the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
