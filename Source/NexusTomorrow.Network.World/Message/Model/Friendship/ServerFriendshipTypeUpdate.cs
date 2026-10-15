using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipTypeUpdate)]
[WireMessage(GameMessageOpcode.ServerFriendshipTypeUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipTypeUpdate : IWritable
{
    [UInt(64)] public ulong FriendshipId { get; set; }
    [UInt(4)] public FriendshipType Type { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(FriendshipId);
        writer.Write(Type, 4u);
    }
}
