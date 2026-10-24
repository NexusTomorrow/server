using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 1: the platform the unit stands on.
/// </summary>
[WireStruct]
[UnionCase(1)]
public sealed partial class SetPlatformCommand : IServer0262Union2
{
    /// <summary>
    /// The unit this unit stands on, by unit id, or 0 when it stands on none: its positions are then in world coordinates. It holds until
    /// the next platform command.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint PlatformUnitId { get; set; }
}
