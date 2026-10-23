using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets the player's death state and shows the resurrect dialog.
/// </summary>
/// <remarks>
/// While the player is dead the client controls the unit given; alive, control returns to the player. Then it raises
/// ShowResurrectDialog.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01B3, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ResurrectDialogShow
{
    /// <summary>
    /// The unit the client controls while the player is dead, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// What a resurrection costs, in money.
    /// </summary>
    [UInt(32)] public uint RezCost { get; set; }

    /// <summary>
    /// The time left on the death penalty, in milliseconds.
    /// </summary>
    [UInt(32)] public uint DeathPenalty { get; set; }

    /// <summary>
    /// True when the player is dead.
    /// </summary>
    [UInt(1)] public bool IsDead { get; set; }

    /// <summary>
    /// The ways the player may resurrect.
    /// </summary>
    [UInt(8)] public ResurrectionType Options { get; set; }

    /// <summary>
    /// True when a caster has offered the player a resurrection.
    /// </summary>
    [UInt(1)] public bool HasCasterRezRequest { get; set; }

    /// <summary>
    /// The time until the player may wake here, in milliseconds.
    /// </summary>
    [UInt(32)] public uint WakeHereCooldown { get; set; }

    /// <summary>
    /// The time until the player is released automatically, in milliseconds.
    /// </summary>
    [UInt(32)] public uint ForceRezTimer { get; set; }
}
