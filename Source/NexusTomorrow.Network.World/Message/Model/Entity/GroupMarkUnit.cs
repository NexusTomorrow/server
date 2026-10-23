using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Places one of the group's target markers on a unit, or takes one off.
/// </summary>
/// <remarks>
/// The client sends it only when the player may mark targets in the group.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0432, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupMarkUnit
{
    /// <summary>
    /// The marker, a TargetMarker table id.
    /// </summary>
    [UInt(32)] public uint MarkerId { get; set; }

    /// <summary>
    /// The unit to mark, by unit id; 0 takes the marker off.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }
}
