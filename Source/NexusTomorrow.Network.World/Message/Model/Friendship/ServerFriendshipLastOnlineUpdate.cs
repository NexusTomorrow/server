using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipLastOnlineUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipLastOnlineUpdate
{
    public Identity PlayerIdentity { get; set; }
    [Float32] public float LastOnlineInDays { get; set; }
}
