using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipAccountLastOnlineUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountLastOnlineUpdate
{
    [UInt(32)] public uint AccountId { get; set; }
    [Float32] public float DaysSinceLastOnline { get; set; }
}
