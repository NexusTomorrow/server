using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

// Clears all items from client's PendingAccountItem list before adding these
[WireMessage(GameMessageOpcode.ServerPendingAccountItemsAdd, Direction.ServerToClient)]
public sealed partial class ServerPendingAccountItemsAdd
{
    [Array(CountBits = 32)] public List<PendingAccountItem> PendingItems { get; set; } = [];
}
