using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(28)]
public sealed partial class Server0262Union2Arm1C : IServer0262Union2
{
    /// <summary>
    /// How many keys the command carries: the number of entries in <see cref="Times"/> and in <see cref="Modes"/>.
    /// </summary>
    [VerifiedMember, UInt(8), Count] public byte Count { get; set; }

    /// <summary>
    /// The time of each key, in milliseconds on the sender's clock. The client maps each through its fit of the sender's clock for the
    /// unit, as it maps <see cref="SetTimeCommand.Time"/>, and the key takes effect at that time.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> Times { get; set; } = [];

    /// <summary>
    /// The unit's movement mode at each key, as <see cref="SetModeCommand.Mode"/> gives one: the mode steps to each at its key's time.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> Modes { get; set; } = [];

    [UInt(32)] public uint Unknown3 { get; set; }
}
