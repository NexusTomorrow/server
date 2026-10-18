using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Tells the server the player has dashed.
/// </summary>
/// <remarks>
/// The client sends it once, when the dash starts, and moves the unit itself. It sends nothing when it refuses the dash for want of
/// energy. While the unit is knocked down it sends <see cref="KnockdownDash"/> instead.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00DE, global::NexusTomorrow.Network.Codec.Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class Dash
{
    /// <summary>
    /// The direction of the dash: 1 forward, 2 backward, 3 left, 4 right.
    /// </summary>
    [UInt(3)] public byte Direction { get; set; }
}
