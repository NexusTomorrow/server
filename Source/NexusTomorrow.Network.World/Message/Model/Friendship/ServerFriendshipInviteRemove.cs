using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipInviteRemove)]
[WireMessage(GameMessageOpcode.ServerFriendshipInviteRemove, Direction.ServerToClient)]
public sealed partial class ServerFriendshipInviteRemove : IWritable
{
    [UInt(64)] public ulong InviteId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(InviteId);
    }
}
