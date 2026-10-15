using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship
{
    // Can also be used to send the player's updated public note
    [Message(GameMessageOpcode.ServerFriendshipAccountPublicNote)]
    public class ServerFriendshipAccountPublicNote : IWritable
    {
        public uint AccountId { get; set; }
        public string PublicNote { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(AccountId);
            writer.WriteStringWide(PublicNote);
        }
    }
}
