using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0976, Direction.ServerToClient)]
public sealed partial class AccountPendingItemAdd
{
    public AccountPendingItem Unknown0 { get; set; } = new();
}
