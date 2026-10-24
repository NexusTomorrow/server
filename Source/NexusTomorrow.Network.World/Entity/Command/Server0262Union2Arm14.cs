using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(20)]
public sealed partial class Server0262Union2Arm14 : IServer0262Union2
{
    /// <summary>
    /// The facing the spin starts from, as <see cref="SetRotationCommand.Rotation"/> gives one, its yaw first.
    /// </summary>
    [VerifiedMember, Float32] public Vector3 Rotation { get; set; }

    /// <summary>
    /// How long the spin lasts, in milliseconds. When it ends, the unit holds the yaw it reached.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Duration { get; set; }

    /// <summary>
    /// How fast the unit's yaw turns, in radians a second, from the yaw of <see cref="Rotation"/>.
    /// </summary>
    [VerifiedMember, PackedFloat] public float SpinRate { get; set; }

    /// <summary>
    /// Time already elapsed in the spin, in milliseconds: the client adds it to the time since the command's start, so the spin starts that
    /// far into it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Offset { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, the client blends away the jump to the segment; when clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
