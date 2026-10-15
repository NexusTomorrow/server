using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipAdd)]
public class ServerFriendshipAdd : IWritable
{
    public FriendData Friend { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        Friend.Write(writer);
    }
}
