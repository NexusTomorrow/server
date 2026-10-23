using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Changes the creature a unit is, and its look.
/// </summary>
[WireMessage(GameMessageOpcode.Server08A8, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class UnitCreatureChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The creature, a Creature2 table id.
    /// </summary>
    [UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// The unit's look, a Creature2DisplayInfo table id.
    /// </summary>
    [UInt(17)] public uint DisplayInfoId { get; set; }

    [UInt(1)] public bool Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }
}
