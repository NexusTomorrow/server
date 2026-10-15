using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    [Message(GameMessageOpcode.ServerFriendshipLevelUpdate)]
    public class ServerFriendshipLevelUpdate : IWritable
    {
        public Identity Character { get; set; }
        public uint Level { get; set; }

        public void Write(GamePacketWriter writer)
        {
            Character.Write(writer);
            writer.Write(Level);
        }
    }
}
