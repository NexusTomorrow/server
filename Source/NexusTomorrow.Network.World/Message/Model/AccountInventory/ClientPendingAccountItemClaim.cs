using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.ClientPendingAccountItemClaim, Direction.ClientToServer)]
public sealed partial class ClientPendingAccountItemClaim
{
    [WideString] public string TransactionId { get; private set; } // used a format like 89d2a447-df3c-4c3c-a135-d4eb8dfa4fd7 but could be anything
}
