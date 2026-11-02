using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets the resurrect dialog's availability and its options.
/// </summary>
[WireMessage(GameMessageOpcode.Server0100, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ResurrectionState
{
    /// <summary>
    /// Whether a resurrection is available; when set, the client runs the resurrection countdown, and when clear it drops any running one.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Available { get; set; }

    /// <summary>
    /// The ways the player may resurrect.
    /// </summary>
    [VerifiedMember, UInt(8)] public ResurrectionType Options { get; set; }
}
