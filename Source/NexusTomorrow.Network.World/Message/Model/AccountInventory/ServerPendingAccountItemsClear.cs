using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

// Clears all items from client's PendingAccountItem list
[WireMessage(GameMessageOpcode.ServerPendingAccountItemsClear, Direction.ServerToClient)]
public sealed partial class ServerPendingAccountItemsClear
{
}
