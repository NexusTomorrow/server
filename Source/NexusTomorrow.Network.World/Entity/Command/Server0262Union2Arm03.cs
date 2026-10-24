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
    /// The time of each key, in milliseconds on the sender's clock. These times place the unit along its keys: where the unit is between
    /// two keys at a given moment depends on how far apart in time the two keys are.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> Times { get; set; } = [];

    /// <summary>
    /// The unit's position at each key: the client moves the unit's position through them.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(Count)), Float32] public List<Vector3> Positions { get; set; } = [];

    /// <summary>
    /// How the unit moves between its keys. With 0 it moves evenly from one key to the next. A value above 1 fails the command, as 1 does
    /// when the command carries fewer than three keys.
    /// </summary>
    [VerifiedMember, UInt(2)] public byte Interpolation { get; set; }

    /// <summary>
    /// Time already elapsed on the command's keys, in milliseconds: the client adds it to the time since the command's start, so it sets
    /// where along its keys the unit is.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Offset { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, on a unit the client does not move, the client blends away the jump to the segment; when
    /// clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
