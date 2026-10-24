using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(23)]
public sealed partial class Server0262Union2Arm17 : IServer0262Union2
{
    /// <summary>
    /// How many keys the command carries: the number of entries in <see cref="Unknown1"/> and in <see cref="Unknown2"/>.
    /// </summary>
    [VerifiedMember, UInt(8), Count] public byte Count { get; set; }

    [Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> Unknown1 { get; set; } = [];

    [Array(CountFrom = nameof(Count)), PackedFloat] public List<float> Unknown2 { get; set; } = [];

    [UInt(2)] public byte Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, the client blends away the jump to the segment; when clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
