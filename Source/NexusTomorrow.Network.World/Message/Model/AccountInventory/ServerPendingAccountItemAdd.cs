using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerPendingAccountItemAdd, Direction.ServerToClient)]
public sealed partial class ServerPendingAccountItemAdd
{
    public PendingAccountItem PendingItem { get; set; }
}
