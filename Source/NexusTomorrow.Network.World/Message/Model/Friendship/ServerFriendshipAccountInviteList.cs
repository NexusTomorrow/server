using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipAccountInviteList, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountInviteList
{
    [WireStruct]
    public sealed partial class FriendAccountInviteInfo
    {
        [UInt(64)] public ulong AccountFriendInviteId { get; set; }
        [UInt(2)] public FriendshipType Type { get; set; }
        [WideString] public string DisplayName { get; set; }
        [WideString] public string Note { get; set; }
        [Float32] public float DaysUntilExpired { get; set; }
        [UInt(32)] public uint Seen { get; set; } // Seems to only be used for flagging seen invites
    }

    [UInt(32)] public uint AccountId { get; set; }
    [Array(CountBits = 32)] public List<FriendAccountInviteInfo> FriendAccountInvites { get; set; } = [];
}
