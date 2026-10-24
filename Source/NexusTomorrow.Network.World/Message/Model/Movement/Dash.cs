using NexusTomorrow.Game.Static.Entity.Movement;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity.Command;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Tells the server the player has dashed.
/// </summary>
/// <remarks>
/// The client sends it once, when the dash starts, and moves the unit itself. It sends nothing when the dash spell's cast check fails,
/// while a dash it sent is still running, in the client's normal dash state while the unit's <see cref="SetModeCommand.Mode"/> is not 0 or
/// bit 0x100 of its <see cref="SetStateCommand.State"/> is clear, or for any unit but the one it moves. In its other dash state it
/// sends only left and right. While the unit has a crowd-control state whose
/// CCStates flags include 0x2 it sends <see cref="KnockdownDash"/> instead.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00DE, global::NexusTomorrow.Network.Codec.Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class Dash
{
    /// <summary>
    /// The direction of the dash.
    /// </summary>
    [VerifiedMember, UInt(3)] public DashDirection Direction { get; set; }
}
