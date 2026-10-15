using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    [Message(GameMessageOpcode.ClientFriendshipRemoveByIdentity)]
    public class ClientFriendshipRemoveByIdentity : IReadable
    {
        public Identity PlayerIdentity { get; private set; } = new();
        public FriendshipType Type { get; private set; }

        public void Read(GamePacketReader reader)
        {
            PlayerIdentity.Read(reader);
            Type = reader.ReadEnum<FriendshipType>(4u);
        }
    }
}
