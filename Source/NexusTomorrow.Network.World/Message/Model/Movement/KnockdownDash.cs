using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Tells the server the player has dashed while knocked down.
/// </summary>
/// <remarks>
/// The client sends it instead of <see cref="Dash"/> while the unit is knocked down, and the dash costs the same.
/// </remarks>
[WireMessage(GameMessageOpcode.Client015C, global::NexusTomorrow.Network.Codec.Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class KnockdownDash
{
    /// <summary>
    /// The direction of the dash: 1 forward, 2 backward, 3 left, 4 right.
    /// </summary>
    [UInt(3)] public byte Direction { get; private set; }
}
