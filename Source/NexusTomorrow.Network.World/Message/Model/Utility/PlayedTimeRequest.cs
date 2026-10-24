using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

/// <summary>
/// Asks for the character's played time. The client sends it, empty, when the player types the /played chat command, and shows the
/// answer, <see cref="PlayedTime"/>, in the chat log.
/// </summary>
[WireMessage(GameMessageOpcode.Client0693, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class PlayedTimeRequest
{
}
