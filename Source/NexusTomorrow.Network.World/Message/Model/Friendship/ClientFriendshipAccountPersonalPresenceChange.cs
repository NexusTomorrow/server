using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipAccountPersonalPresenceChange, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountPersonalPresenceChange
{
    [UInt(3)] public AccountPresenceState Presence { get; private set; }
}
