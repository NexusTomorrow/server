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

    /// <summary>
    /// Where the unit's flight starts. On a unit the client already shows, the client starts the flight from the unit's shown position
    /// instead.
    /// </summary>
    [VerifiedMember, Float32] public Vector3 StartPosition { get; set; }

    /// <summary>
    /// How long the flight lasts, in milliseconds, before the unit reaches <see cref="TargetPosition"/>.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint FlightTime { get; set; }

    /// <summary>
    /// How far the flight bows above the straight line from its start to <see cref="TargetPosition"/>: a larger value bows higher, and
    /// zero or a negative value does not rise above the line.
    /// </summary>
    [VerifiedMember, Float32] public float Gravity { get; set; }

    [UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, on a unit the client does not move, the client blends away the jump to the segment; when
    /// clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
