using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 2: the unit's position.
/// </summary>
[WireStruct]
[UnionCase(2)]
public sealed partial class SetPositionCommand : IServer0262Union2
{
    /// <summary>
    /// The position, in world coordinates while <see cref="SetPlatformCommand.PlatformUnitId"/> is 0.
    /// </summary>
    [VerifiedMember, Float32] public Vector3 Position { get; set; }

    /// <summary>
    /// Whether the value blends into the unit's motion. When set and the newest segment on this command's channel is a keyed one that this
    /// command made, the value becomes that segment's next key; otherwise the value starts a new segment, and on a unit the client does not
    /// move the client blends away the jump to it. When clear, the value starts a new segment and the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
