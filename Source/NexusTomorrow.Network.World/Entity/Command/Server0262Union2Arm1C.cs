using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(28)]
public sealed partial class Server0262Union2Arm1C : IServer0262Union2
{
    /// <summary>
    /// How many keys the command carries: the number of entries in <see cref="Unknown1"/> and in <see cref="Modes"/>.
    /// </summary>
    [VerifiedMember, UInt(8), Count] public byte Count { get; set; }

    [Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> Unknown1 { get; set; } = [];

    /// <summary>
    /// The unit's movement mode at each key, as <see cref="SetModeCommand.Mode"/> gives one.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> Modes { get; set; } = [];

    [UInt(32)] public uint Unknown3 { get; set; }
}
