using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipInviteRemove)]
public class ServerFriendshipInviteRemove : IWritable
{
    public ulong InviteId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(InviteId);
    }
}
