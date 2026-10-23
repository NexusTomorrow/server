using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Adds a unit to a soldier event under its type, with that type's maximum health.
/// </summary>
/// <remarks>
/// The client adds the unit and type to the event's units, which SoldierEvent:GetDefendUnits, GetAuxiliaryUnits and GetEscapingUnits list
/// by type, and raises UpdateSoldierBuild. <see cref="SoldierEventUnitRemove"/> takes the pair out.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0678, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SoldierEventUnitUpdate
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

    /// <summary>
    /// The maximum health of the event's units of this type: SoldierEvent:GetMaxDefendHealth returns it for defend units and
    /// GetMaxAuxiliaryHealth for auxiliary ones. The client keeps none for escaping units.
    /// </summary>
    [Float32] public float MaxHealth { get; set; }
}
