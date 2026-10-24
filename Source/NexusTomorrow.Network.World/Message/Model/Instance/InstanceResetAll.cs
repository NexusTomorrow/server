using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Asks to reset the player's instances: the request a chat command sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client07B4, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class InstanceResetAll
{
}
