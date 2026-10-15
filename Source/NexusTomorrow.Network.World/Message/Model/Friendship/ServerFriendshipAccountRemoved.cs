using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    [Message(GameMessageOpcode.ServerFriendshipAccountRemoved)]
    public class ServerFriendshipAccountRemoved : IWritable
    {
        public ulong AccountFriendId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(AccountFriendId);
        }
    }
}
