using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientGroupRequestJoin, Direction.ClientToServer)]
public sealed partial class ClientGroupRequestJoin
{
    [WideString] public string Name { get; set; }
    [WideString] public string RealmName { get; set; }
}
