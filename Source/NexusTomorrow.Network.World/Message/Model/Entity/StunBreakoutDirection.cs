using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Starts a stun breakout: the direction the player presses to break free.
/// </summary>
/// <remarks>
/// The client raises ActivateCCStateStun with the direction and starts its breakout progress over.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00C4, NexusTomorrow.Network.Codec.Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class StunBreakoutDirection
{
    /// <summary>
    /// The direction to press.
    /// </summary>
    [UInt(8)] public CCStateStunVictimGameplay Direction { get; set; }
}
