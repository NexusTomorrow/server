using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// One unit of a soldier event with its type: the pair <see cref="SoldierEventUnitUpdate"/> adds.
/// </summary>
[WireStruct]
public sealed partial class SoldierEventUnit
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's type in the event.
    /// </summary>
    [UInt(32)] public SoldierEventUnitType UnitType { get; set; }
}
