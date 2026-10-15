using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.ClientAccountItemGiftReturn, Direction.ClientToServer)]
public sealed partial class ClientAccountItemGiftReturn
{
    [WideString] public string TransactionId { get; private set; }
}
