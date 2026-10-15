using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipList, Direction.ServerToClient)]
public sealed partial class ServerFriendshipList
{
    [Array(CountBits = 16)] public List<FriendData> Friends { get; set; } = [];
}
