using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

/// <summary>
/// Confirms the player is leaving the world.
/// </summary>
/// <remarks>
/// The client sends it once while a request is pending: when the countdown from <see cref="LogoutCountdown"/> ends, or at once
/// when the player confirms.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00C0, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class LogoutConfirm
{
}
