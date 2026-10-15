using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

// Can be used for AccountFriends or the player when they update their account display name
[WireMessage(GameMessageOpcode.ServerFriendshipAccountDisplayNameUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountDisplayNameUpdate
{
    [UInt(32)] public uint AccountId { get; set; }
    [WideString] public string DisplayName { get; set; }
}
