using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

/// <summary>
/// Asks to leave the world, or cancels a request still waiting.
/// </summary>
/// <remarks>
/// The request is the same however the player chose to leave. The client sends <see cref="LogoutConfirm"/> when the countdown
/// from <see cref="LogoutCountdown"/> ends.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00BF, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class LogoutRequest
{
    /// <summary>
    /// True asks to leave the world; false cancels a request still counting down.
    /// </summary>
    [UInt(1)] public bool Begin { get; private set; }
}
