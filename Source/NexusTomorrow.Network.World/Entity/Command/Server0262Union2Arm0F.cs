using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(15)]
public sealed partial class Server0262Union2Arm0F : IServer0262Union2
{
    /// <summary>
    /// How many keys the command carries: the number of entries in <see cref="Times"/> and in <see cref="Rotations"/>.
    /// </summary>
    [VerifiedMember, UInt(10), Count] public ushort Count { get; set; }

    /// <summary>
    /// The time of each key, in milliseconds on the sender's clock. The client maps each through its fit of the sender's clock for the
    /// unit, as it maps <see cref="SetTimeCommand.Time"/>, and the key takes effect at that time.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> Times { get; set; } = [];

    /// <summary>
    /// The unit's facing at each key, as <see cref="SetRotationCommand.Rotation"/> gives one: the client turns the unit through them by
    /// the keys' times.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(Count)), Float32] public List<Vector3> Rotations { get; set; } = [];

    [UInt(2)] public byte Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, the client blends away the jump to the segment; when clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
