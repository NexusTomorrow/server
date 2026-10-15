using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipAdd, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAdd
{
    public FriendData Friend { get; set; } = new();
}
