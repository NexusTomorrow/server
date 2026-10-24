using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(18)]
public sealed partial class Server0262Union2Arm12 : IServer0262Union2
{
    /// <summary>
    /// The unit to face, by unit id: the client finds that unit and turns this one toward it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, the client blends away the jump to the segment; when clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
