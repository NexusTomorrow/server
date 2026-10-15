using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Not captured in any sniffs so maybe was not needed
[WireMessage(GameMessageOpcode.ServerDialogEnd, Direction.ServerToClient)]
public sealed partial class ServerDialogEnd
{
}
