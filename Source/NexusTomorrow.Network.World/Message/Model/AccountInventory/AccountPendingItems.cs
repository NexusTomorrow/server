using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0979, Direction.ServerToClient)]
public sealed partial class AccountPendingItems
{
    [Array(CountBits = 32)] public List<AccountPendingItem> Unknown0 { get; set; } = [];
}
