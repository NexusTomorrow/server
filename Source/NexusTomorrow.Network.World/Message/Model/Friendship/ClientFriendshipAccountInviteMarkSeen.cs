using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    [Message(GameMessageOpcode.ClientFriendshipAccountInviteMarkSeen)]
    public class ClientFriendshipAccountInviteMarkSeen : IReadable
    {
        public ulong AccountFriendInviteId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            AccountFriendInviteId = reader.ReadULong();
        }
    }
}
