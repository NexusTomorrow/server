using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Client07C6, Direction.ClientToServer)]
public sealed partial class AccountPendingItemGroupReturn
{
    [WideString] public string Unknown0 { get; private set; } = "";
}
