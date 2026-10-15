using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.ClientAccountItemGiftByIdentity, Direction.ClientToServer)]
public sealed partial class ClientAccountItemGiftByIdentity
{
    [WideString] public string TransactionId { get; private set; }
    public Identity Recipient { get; private set; } = new();
}
