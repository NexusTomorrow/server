using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 24: the unit's movement state.
/// </summary>
[WireStruct]
[UnionCase(24)]
public sealed partial class SetStateCommand : IServer0262Union2
{
    /// <summary>
    /// The unit's movement state, as bits. It holds until the next state command. In the state the client derives from a unit's channels,
    /// bit 0x1 is set while <see cref="SetVelocityCommand.Velocity"/> is zero and bit 0x2 while
    /// <see cref="SetMoveCommand.CommandedVelocity"/> is nonzero.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint State { get; set; }
}
