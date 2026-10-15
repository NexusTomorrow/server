using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountItems, Direction.ServerToClient)]
public sealed partial class ServerAccountItems
{
    [Array(CountBits = 32)] public List<AccountInventoryItem> AccountItems { get; set; } = [];
}
