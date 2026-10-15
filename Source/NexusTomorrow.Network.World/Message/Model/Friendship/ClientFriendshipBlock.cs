using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    [Message(GameMessageOpcode.ClientFriendshipBlock)]
    public class ClientFriendshipBlock : IReadable
    {
        public bool BlockFriendRequests { get; private set; } 

        public void Read(GamePacketReader reader)
        {
            BlockFriendRequests = reader.ReadBit();
        }
    }
}
