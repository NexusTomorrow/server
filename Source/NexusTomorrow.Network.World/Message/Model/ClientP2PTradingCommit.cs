using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientP2PTradingCommit, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingCommit
{
}
