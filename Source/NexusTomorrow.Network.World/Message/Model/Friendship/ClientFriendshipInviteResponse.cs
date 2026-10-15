using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipInviteResponse)]
[WireMessage(GameMessageOpcode.ClientFriendshipInviteResponse, Direction.ClientToServer)]
public sealed partial class ClientFriendshipInviteResponse : IReadable
{
    [UInt(64)] public ulong InviteId { get; private set; }
    [UInt(3)] public FriendshipResponse Response { get; private set; }

    public void Read(GamePacketReader reader)
    {
        InviteId = reader.ReadULong();
        Response = reader.ReadEnum<FriendshipResponse>(3u);
    }
}
