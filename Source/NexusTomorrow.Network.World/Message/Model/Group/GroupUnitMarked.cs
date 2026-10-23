using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Places one of a group's target markers on a unit, or clears it.
/// </summary>
/// <remarks>
/// For its current or previous group the client moves the marker and raises Group_SetMark.
/// </remarks>
[WireMessage(GameMessageOpcode.Server045B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupUnitMarked
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The marker, a TargetMarker table id.
    /// </summary>
    [UInt(32)] public uint MarkerId { get; set; }

    /// <summary>
    /// The unit marked, by unit id; 0 clears the marker.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }
}
