using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 21: hands the unit's facing to its motion, so the unit faces the way it moves.
/// </summary>
[WireStruct]
[UnionCase(21)]
public sealed partial class Server0262Union2Arm15 : IServer0262Union2
{
    /// <summary>
    /// Whether the segment blends in: when set, the client blends away the jump to the segment; when clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
