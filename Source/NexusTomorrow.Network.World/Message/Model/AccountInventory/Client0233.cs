using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.Client0233, Direction.ClientToServer)]
public sealed partial class Client0233
{
    [WideString] public string Unknown0 { get; private set; }
}
