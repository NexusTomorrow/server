using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipRemove, Direction.ServerToClient)]
public sealed partial class ServerFriendshipRemove
{
    [UInt(64)] public ulong FriendshipId { get; set; }
}
