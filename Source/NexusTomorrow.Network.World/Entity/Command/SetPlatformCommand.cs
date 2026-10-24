using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(1)]
public sealed partial class SetPlatformCommand : IServer0262Union2
{
    /// <summary>
    /// Zero when the unit stands on no platform: its positions are then in world coordinates.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown0 { get; set; }
}
