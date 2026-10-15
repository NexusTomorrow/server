using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.ClientCreddHistoryRequest, Direction.ClientToServer)]
public sealed partial class ClientCreddHistoryRequest
{
}
