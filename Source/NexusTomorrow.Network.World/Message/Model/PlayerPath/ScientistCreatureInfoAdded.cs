using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Files a scientist creature info entry under a unit's creature.
/// </summary>
[WireMessage(GameMessageOpcode.Server08B2, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ScientistCreatureInfoAdded
{
    /// <summary>
    /// The unit, by unit id; the client files the entry under the unit's creature.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The entry, a PathScientistCreatureInfo table id.
    /// </summary>
    [UInt(14)] public uint CreatureInfoId { get; set; }
}
