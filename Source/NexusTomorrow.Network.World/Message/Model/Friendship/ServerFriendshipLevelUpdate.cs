using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipLevelUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipLevelUpdate
{
    public Identity Character { get; set; }
    [UInt(32)] public uint Level { get; set; }
}
