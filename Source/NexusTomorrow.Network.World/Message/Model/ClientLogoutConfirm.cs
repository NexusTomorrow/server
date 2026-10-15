using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Called from ExitNow and ConfirmCamp lua functions
[WireMessage(GameMessageOpcode.ClientLogoutConfirm, Direction.ClientToServer)]
public sealed partial class ClientLogoutConfirm
{
}
