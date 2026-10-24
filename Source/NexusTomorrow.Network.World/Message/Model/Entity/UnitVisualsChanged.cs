using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Changes a unit's look: its race, gender, creature, display, outfit and the items it shows.
/// </summary>
/// <remarks>
/// When the unit is the player, the client also raises AppearanceChanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0905, Direction.ServerToClient)]
[Verified(Rung.Fields)]
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

    /// <summary>
    /// The unit's outfit, as an id in the client's Creature2OutfitInfo table, or 0 for none. The client ignores an id the table lacks.
    /// </summary>
    [UInt(15)] public ushort OutfitInfoId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [Float32] public float Unknown6 { get; set; }

    /// <summary>
    /// Whether this look is laid over the unit's own. Set, the unit keeps its own race and gender, its own bone values stop applying, and
    /// a cinematic copy of the unit carries the look. Clear, the race and gender also become the unit's own.
    /// </summary>
    [UInt(1)] public bool IsTemporaryAppearance { get; set; }

    /// <summary>
    /// The items the unit shows, each giving one slot of its model its look and colour as <see cref="UnitItemVisualsChanged.Items"/>
    /// does.
    /// </summary>
    [Array(CountBits = 32)] public List<ItemVisual> Items { get; set; } = [];
}
