using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets group target markers: the client accepts it when the id matches one of its two groups, applies the marks to its first group,
/// and raises Group_SetMark for a mark only when that group is the current one and the TargetMarker table holds the marker.
/// </summary>
[WireMessage(GameMessageOpcode.Server0431, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupMarksChanged
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The marks to apply, each a marker and the unit it marks.
    /// </summary>
    [Array(CountBits = 32)] public List<GroupMark> Marks { get; } = [];
}
