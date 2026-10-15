using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    // Can be used for AccountFriends or the player when they update their account display name
    [Message(GameMessageOpcode.ServerFriendshipAccountDisplayNameUpdate)]
    public class ServerFriendshipAccountDisplayNameUpdate : IWritable
    {
        public uint AccountId { get; set; }
        public string DisplayName { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(AccountId);
            writer.WriteStringWide(DisplayName);
        }
    }
}
