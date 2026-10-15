using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipAccountPersonalPresenceChange)]
[WireMessage(GameMessageOpcode.ClientFriendshipAccountPersonalPresenceChange, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountPersonalPresenceChange : IReadable
{
    [UInt(3)] public AccountPresenceState Presence { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Presence = reader.ReadEnum<AccountPresenceState>(3u);
    }
}
