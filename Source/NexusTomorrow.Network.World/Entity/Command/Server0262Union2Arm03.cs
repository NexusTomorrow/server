using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(3)]
public sealed partial class Server0262Union2Arm03 : IServer0262Union2
{
    /// <summary>
    /// How many keys the command carries: the number of entries in <see cref="Times"/> and in <see cref="Positions"/>.
    /// </summary>
    [VerifiedMember, UInt(10), Count] public ushort Count { get; set; }

    /// <summary>
    /// The time of each key, in milliseconds on the sender's clock. The client maps each through its fit of the sender's clock for the
    /// unit, as it maps <see cref="SetTimeCommand.Time"/>, and the key takes effect at that time.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> Times { get; set; } = [];

    /// <summary>
    /// The unit's position at each key: the client moves the unit's position through them by the keys' times.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(Count)), Float32] public List<Vector3> Positions { get; set; } = [];

    [UInt(2)] public byte Unknown3 { get; set; }

    /// <summary>
    /// Time already elapsed on the command's keys, in milliseconds: the client adds it to the time since the command's start, so it sets
    /// where along its keys the unit is.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint TimeOffset { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, on a unit the client does not move, the client blends away the jump to the segment; when
    /// clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
