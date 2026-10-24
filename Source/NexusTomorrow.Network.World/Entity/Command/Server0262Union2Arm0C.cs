using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(12)]
public sealed partial class Server0262Union2Arm0C : IServer0262Union2
{
    /// <summary>
    /// How many keys the command carries: the number of entries in <see cref="Times"/> and in <see cref="CommandedVelocities"/>.
    /// </summary>
    [VerifiedMember, UInt(10), Count] public ushort Count { get; set; }

    /// <summary>
    /// The time of each key, in milliseconds on the sender's clock. The client maps each through its fit of the sender's clock for the
    /// unit, as it maps <see cref="SetTimeCommand.Time"/>, and the key takes effect at that time.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> Times { get; set; } = [];

    /// <summary>
    /// The velocity the unit is driven at, at each key, as <see cref="SetMoveCommand.CommandedVelocity"/> gives one: the client moves that
    /// velocity through them by the keys' times.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(Count)), PackedFloat] public List<Vector3> CommandedVelocities { get; set; } = [];

    [UInt(2)] public byte Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, the client blends away the jump to the segment; when clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
