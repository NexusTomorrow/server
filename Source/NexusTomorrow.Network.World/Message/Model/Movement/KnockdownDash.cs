using NexusTomorrow.Game.Static.Entity.Movement;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Tells the server the player has dashed while held by a crowd-control state whose CCStates flags include 0x2.
/// </summary>
/// <remarks>
/// The client sends it instead of <see cref="Dash"/> while the unit has a crowd-control state whose CCStates flags include 0x2, knockdown
/// among them, and the dash costs the same.
/// </remarks>
[WireMessage(GameMessageOpcode.Client015C, global::NexusTomorrow.Network.Codec.Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class KnockdownDash
{
    /// <summary>
    /// The direction of the dash, as <see cref="Dash.Direction"/> gives it.
    /// </summary>
    [VerifiedMember, UInt(3)] public DashDirection Direction { get; private set; }
}
