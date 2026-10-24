using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Tells the server the unit the client moves has landed hard.
/// </summary>
/// <remarks>
/// The client sends it after a fall's landing, not after a small jump, and sends the unit's movement (<see cref="ClientMovement"/>) just
/// before it.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00FB, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class FallingDamage
{
    [Float32] public float Unknown0 { get; set; }
}
