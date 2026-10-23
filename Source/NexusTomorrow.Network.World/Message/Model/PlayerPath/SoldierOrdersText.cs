using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Has a unit show a soldier mission's orders.
/// </summary>
/// <remarks>
/// The client acts only for a unit tied to that mission. It shows the mission's orders text (its localizedTextIdSoldierOrders, or a line
/// of its own when the row has none), with the unit and the player as the target, in the unit's speech bubble.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0677, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SoldierOrdersText
{
    /// <summary>
    /// The mission, a PathMission table id.
    /// </summary>
    [UInt(15)] public ushort MissionId { get; set; }

    /// <summary>
    /// The unit that speaks, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }
}
