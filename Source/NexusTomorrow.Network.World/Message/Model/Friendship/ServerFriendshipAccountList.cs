using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipAccountList, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountList
{
    [WireStruct]
    public sealed partial class AccountFriend
    {
        [UInt(32)] public uint AccountId { get; set; }
        [UInt(64)] public ulong AccountFriendId { get; set; }
        [WideString] public string PublicNote { get; set; } = "";
        [Float32] public float DaysSinceLastLogin { get; set; }
        [WideString] public string PrivateNote { get; set; } = "";
        [WideString] public string DisplayName { get; set; } = "";
        [UInt(3)] public AccountPresenceState Presence { get; set; }
        [Array(CountBits = 32)] public List<CharacterData> CharacterList { get; set; } = [];
    }

    [Array(CountBits = 32)] public List<AccountFriend> FriendListData { get; set; } = [];
}
