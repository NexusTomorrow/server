using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Asks for the player's raid lockouts: the request a chat command sends.
/// </summary>
/// <remarks>
/// The server answers with <see cref="Group.RaidLockouts"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0719, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class RaidInfoRequest
{
}
