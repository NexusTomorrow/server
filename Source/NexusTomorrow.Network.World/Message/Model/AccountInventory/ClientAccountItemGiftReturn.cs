using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.Client07C6, Direction.ClientToServer)]
public sealed partial class Client07C6
{
    [WideString] public string Unknown0 { get; private set; }
}
