using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipRemove)]
[WireMessage(GameMessageOpcode.ServerFriendshipRemove, Direction.ServerToClient)]
public sealed partial class ServerFriendshipRemove : IWritable
{
    [UInt(64)] public ulong FriendshipId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(FriendshipId);
    }
}
