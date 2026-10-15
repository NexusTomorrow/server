using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

// Not seen in sniffs but there is lua UI code for handling the triggered event
[WireMessage(GameMessageOpcode.ServerShowInstanceRestrictedDialog, Direction.ServerToClient)]
public sealed partial class ServerShowInstanceRestrictedDialog
{
}
