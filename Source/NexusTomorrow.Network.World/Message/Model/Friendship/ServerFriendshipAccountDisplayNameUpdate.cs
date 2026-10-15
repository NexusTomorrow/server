using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

// Can be used for AccountFriends or the player when they update their account display name
[Message(GameMessageOpcode.ServerFriendshipAccountDisplayNameUpdate)]
[WireMessage(GameMessageOpcode.ServerFriendshipAccountDisplayNameUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountDisplayNameUpdate : IWritable
{
    [UInt(32)] public uint AccountId { get; set; }
    [WideString] public string DisplayName { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountId);
        writer.WriteStringWide(DisplayName);
    }
}
