using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(22)]
public sealed partial class Server0262Union2Arm16 : IServer0262Union2
{
    [PackedFloat] public float Unknown0 { get; set; }

    /// <summary>
    /// Whether the value blends into the unit's scale. When set and the newest segment on this command's channel is a keyed one that this
    /// command made, the value becomes that segment's next key; otherwise the value starts a new segment, and the client blends away the
    /// jump to it. When clear, the value starts a new segment and the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
