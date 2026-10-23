using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03C7, Direction.ClientToServer)]
public sealed partial class FriendshipSetNoteByIdentity
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    [WideString] public string Unknown1 { get; private set; } = "";
}
