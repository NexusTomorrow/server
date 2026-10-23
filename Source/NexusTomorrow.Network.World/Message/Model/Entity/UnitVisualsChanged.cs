using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Changes a unit's look: its race, gender, creature and display.
/// </summary>
[WireMessage(GameMessageOpcode.Server0905, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class UnitVisualsChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's race.
    /// </summary>
    [UInt(5)] public Race Race { get; set; }

    /// <summary>
    /// The unit's gender.
    /// </summary>
    [UInt(2)] public Sex Gender { get; set; }

    /// <summary>
    /// The creature, a Creature2 table id.
    /// </summary>
    [UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// The unit's look, a Creature2DisplayInfo table id.
    /// </summary>
    [UInt(17)] public uint DisplayInfoId { get; set; }

    [UInt(15)] public ushort Unknown5 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown6 { get; set; }

    [UInt(1)] public bool Unknown7 { get; set; }

    [Array(CountBits = 32)] public List<Server0117Part1> Unknown8 { get; set; } = [];
}
