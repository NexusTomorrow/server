using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipAccountCharacterLevelUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountCharacterLevelUpdate
{
    [UInt(32)] public uint AccountId { get; set; }
    public Identity Character { get; set; }
    [UInt(8)] public uint Level { get; set; }
}
