using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipList)]
[WireMessage(GameMessageOpcode.ServerFriendshipList, Direction.ServerToClient)]
public sealed partial class ServerFriendshipList : IWritable
{
    [Array(CountBits = 16)] public List<FriendData> Friends { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Friends.Count, 16u);
        Friends.ForEach(f => f.Write(writer));
    }
}
