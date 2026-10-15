using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    [Message(GameMessageOpcode.ServerFriendshipList)]
    public class ServerFriendshipList : IWritable
    {
        public List<FriendData> Friends { get; set; } = [];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Friends.Count, 16u);
            Friends.ForEach(f => f.Write(writer));
        }
    }
}
