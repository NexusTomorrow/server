using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets the resurrect dialog's options.
/// </summary>
[WireMessage(GameMessageOpcode.Server0100, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ResurrectionState
{
    [UInt(1)] public bool Unknown0 { get; set; }

    /// <summary>
    /// The ways the player may resurrect.
    /// </summary>
    [UInt(8)] public ResurrectionType Options { get; set; }
}
