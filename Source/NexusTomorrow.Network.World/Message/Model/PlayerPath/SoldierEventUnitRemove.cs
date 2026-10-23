using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Takes a unit of a type out of a soldier event.
/// </summary>
/// <remarks>
/// The client erases the unit and type pair <see cref="SoldierEventUnitUpdate"/> adds, and raises UpdateSoldierBuild.
/// </remarks>
[WireMessage(GameMessageOpcode.Server067A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SoldierEventUnitRemove
{
    /// <summary>
    /// The soldier event, a PathSoldierEvent table id.
    /// </summary>
    [UInt(14)] public ushort SoldierEventId { get; set; }

    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's type in the event.
    /// </summary>
    [UInt(32)] public SoldierEventUnitType UnitType { get; set; }
}
