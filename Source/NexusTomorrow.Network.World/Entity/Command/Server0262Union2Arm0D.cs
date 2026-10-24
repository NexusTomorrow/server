using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 13: returns the unit's commanded velocity to its default.
/// </summary>
[WireStruct]
[UnionCase(13)]
public sealed partial class Server0262Union2Arm0D : IServer0262Union2
{
    /// <summary>
    /// Kept with the segment the command starts: the command always starts a new segment on its channel, and the client keeps the bit
    /// with the segment.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
