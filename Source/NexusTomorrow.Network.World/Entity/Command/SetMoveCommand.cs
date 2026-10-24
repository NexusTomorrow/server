using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 11: the velocity the unit is driven at.
/// </summary>
[WireStruct]
[UnionCase(11)]
public sealed partial class SetMoveCommand : IServer0262Union2
{
    /// <summary>
    /// The velocity the unit is driven at, which <see cref="SetVelocityCommand.Velocity"/> follows. A position segment that starts takes it
    /// as its own velocity. In the state the client derives from a unit's channels, bit 0x2 of <see cref="SetStateCommand.State"/> is set
    /// while this is nonzero, and the client sets the same bit in the state it sends while input drives the unit it moves.
    /// </summary>
    [VerifiedMember, PackedFloat] public Vector3 CommandedVelocity { get; set; }

    /// <summary>
    /// Whether the value extends a keyed segment. When set and the newest segment on this command's channel is a keyed one that this
    /// command made, the value becomes that segment's next key; otherwise the value starts a new segment. The client keeps the bit with
    /// the segment.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
