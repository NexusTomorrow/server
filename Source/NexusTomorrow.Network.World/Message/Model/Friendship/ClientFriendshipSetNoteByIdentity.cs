using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipSetNoteByIdentity, Direction.ClientToServer)]
public sealed partial class ClientFriendshipSetNoteByIdentity
{
    public Identity PlayerIdentity { get; private set; } = new();
    [WideString] public string Note { get; private set; }
}
