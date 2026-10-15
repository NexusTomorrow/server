using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientGroupInvite, Direction.ClientToServer)]
public sealed partial class ClientGroupInvite
{
    [WideString] public string Name { get; set; }
    [WideString] public string RealmName { get; set; }
}
