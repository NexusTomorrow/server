using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipInviteResponse)]
public class ClientFriendshipInviteResponse : IReadable
{
    public ulong InviteId { get; private set; }
    public FriendshipResponse Response { get; private set; }

    public void Read(GamePacketReader reader)
    {
        InviteId = reader.ReadULong();
        Response = reader.ReadEnum<FriendshipResponse>(3u);
    }
}
