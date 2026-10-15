using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipSetNoteByName, Direction.ClientToServer)]
public sealed partial class ClientFriendshipSetNoteByName
{
    [WideString] public string Name { get; private set; }
    [WideString] public string RealmName { get; private set; }
    [WideString] public string Note { get; private set; }
}
