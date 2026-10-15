using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    // Can also be used to send the player's updated private note
    [Message(GameMessageOpcode.ServerFriendshipAccountPrivateNote)]
    public class ServerFriendshipAccountPrivateNote : IWritable
    {
        public uint AccountId { get; set; }
        public string PrivateNote { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(AccountId);
            writer.WriteStringWide(PrivateNote);
        }
    }
}
