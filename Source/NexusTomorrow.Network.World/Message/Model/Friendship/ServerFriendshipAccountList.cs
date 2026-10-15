using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipAccountList)]
[WireMessage(GameMessageOpcode.ServerFriendshipAccountList, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountList : IWritable
{
    [WireStruct]
    public sealed partial class AccountFriend : IWritable
    {
        [UInt(32)] public uint AccountId { get; set; }
        [UInt(64)] public ulong AccountFriendId { get; set; }
        [WideString] public string PublicNote { get; set; } = "";
        [Float32] public float DaysSinceLastLogin { get; set; }
        [WideString] public string PrivateNote { get; set; } = "";
        [WideString] public string DisplayName { get; set; } = "";
        [UInt(3)] public AccountPresenceState Presence { get; set; }
        [Array(CountBits = 32)] public List<CharacterData> CharacterList { get; set; } = [];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(AccountId);
            writer.Write(AccountFriendId);
            writer.WriteStringWide(PublicNote);
            writer.Write(DaysSinceLastLogin);
            writer.WriteStringWide(PrivateNote);
            writer.WriteStringWide(DisplayName);
            writer.Write(Presence, 3u);

            writer.Write(CharacterList.Count, 32u);
            CharacterList.ForEach(c => c.Write(writer));
        }
    }

    [Array(CountBits = 32)] public List<AccountFriend> FriendListData { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(FriendListData.Count, 32u);
        FriendListData.ForEach(f => f.Write(writer));
    }
}
