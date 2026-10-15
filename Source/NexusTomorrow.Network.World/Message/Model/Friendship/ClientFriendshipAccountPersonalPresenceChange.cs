using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    [Message(GameMessageOpcode.ClientFriendshipAccountPersonalPresenceChange)]
    public class ClientFriendshipAccountPersonalPresenceChange : IReadable
    {
        public AccountPresenceState Presence { get; private set; }

        public void Read(GamePacketReader reader)
        {
            Presence = reader.ReadEnum<AccountPresenceState>(3u);
        }
    }
}
