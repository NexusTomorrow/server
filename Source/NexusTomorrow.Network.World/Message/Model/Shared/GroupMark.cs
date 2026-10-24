using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// One of a group's target markers and the unit it marks.
/// </summary>
[WireStruct]
public sealed partial class GroupMark
{
    /// <summary>
    /// The marker, a TargetMarker table id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint MarkerId { get; set; }

    /// <summary>
    /// The unit marked, by unit id; 0 clears the marker.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }
}
