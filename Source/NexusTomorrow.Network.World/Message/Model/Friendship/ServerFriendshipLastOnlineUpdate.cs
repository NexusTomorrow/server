using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipLastOnlineUpdate)]
[WireMessage(GameMessageOpcode.ServerFriendshipLastOnlineUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipLastOnlineUpdate : IWritable
{
    public Identity PlayerIdentity { get; set; }
    [Float32] public float LastOnlineInDays { get; set; }

    public void Write(GamePacketWriter writer)
    {
        PlayerIdentity.Write(writer);
        writer.Write(LastOnlineInDays);
    }
}
