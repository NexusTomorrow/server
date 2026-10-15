using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipAccountInviteRemove)]
[WireMessage(GameMessageOpcode.ServerFriendshipAccountInviteRemove, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountInviteRemove : IWritable
{
    [UInt(32)] public uint AccountId { get; set; }
    [UInt(64)] public ulong AccountFriendInviteId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountId);
        writer.Write(AccountFriendInviteId);
    }
}
