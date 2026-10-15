using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    [Message(GameMessageOpcode.ServerFriendshipLastOnlineUpdate)]
    public class ServerFriendshipLastOnlineUpdate : IWritable
    {
        public Identity PlayerIdentity { get; set; }
        public float LastOnlineInDays { get; set; }

        public void Write(GamePacketWriter writer)
        {
            PlayerIdentity.Write(writer);
            writer.Write(LastOnlineInDays);
        }
    }
}
