using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    [Message(GameMessageOpcode.ServerFriendshipRemove)]
    public class ServerFriendshipRemove : IWritable
    {
        public ulong FriendshipId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(FriendshipId);
        }
    }
}
