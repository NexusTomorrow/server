using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(10)]
public sealed partial class Server0262Union2Arm0A : IServer0262Union2
{
    /// <summary>
    /// Whether the segment blends in: when set, the client blends away the jump to the segment; when clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
