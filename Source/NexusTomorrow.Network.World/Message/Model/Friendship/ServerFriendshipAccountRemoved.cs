using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipAccountRemoved, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountRemoved
{
    [UInt(64)] public ulong AccountFriendId { get; set; }
}
