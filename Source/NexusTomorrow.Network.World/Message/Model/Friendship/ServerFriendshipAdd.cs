using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipAdd)]
[WireMessage(GameMessageOpcode.ServerFriendshipAdd, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAdd : IWritable
{
    public FriendData Friend { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        Friend.Write(writer);
    }
}
