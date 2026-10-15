using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipAccountPersonalStatus)]
[WireMessage(GameMessageOpcode.ServerFriendshipAccountPersonalStatus, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountPersonalStatus : IWritable
{
    [WideString] public string AccountPublicStatus { get; set; }
    [WideString] public string AccountNickname { get; set; }
    [UInt(3)] public AccountPresenceState Presence { get; set; }
    [UInt(1)] public bool BlockStrangerRequests { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteStringWide(AccountPublicStatus);
        writer.WriteStringWide(AccountNickname);
        writer.Write(Presence, 3);
        writer.Write(BlockStrangerRequests);
    }
}
