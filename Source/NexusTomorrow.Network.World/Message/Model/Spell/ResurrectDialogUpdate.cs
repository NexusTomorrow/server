using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Updates the resurrect dialog's options.
/// </summary>
/// <remarks>
/// The client raises UpdateResurrectDialog while the player is dead.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01AB, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ResurrectDialogUpdate
{
    /// <summary>
    /// The ways the player may resurrect.
    /// </summary>
    [UInt(8)] public ResurrectionType Options { get; set; }

    /// <summary>
    /// True when a caster has offered the player a resurrection.
    /// </summary>
    [UInt(1)] public bool HasCasterRezRequest { get; set; }
}
