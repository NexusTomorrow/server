using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Asks to leave an instance the player is about to be removed from: the request GameLib.LeavePendingRemovalInstance sends.
/// </summary>
/// <remarks>
/// The client sends it only while the removal warning's timer runs.
/// </remarks>
[WireMessage(GameMessageOpcode.Client057C, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class PendingRemovalInstanceLeave
{
}
