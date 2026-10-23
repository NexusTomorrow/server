using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets the group a unit belongs to.
/// </summary>
/// <remarks>
/// The client raises UnitGroupChanged with the unit; Unit.IsInYourGroup compares the unit's group with the player's current group.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0936, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitGroupChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's group, by id; 0 when it is in none.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }
}
