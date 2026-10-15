using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipAccountInviteMarkSeen)]
[WireMessage(GameMessageOpcode.ClientFriendshipAccountInviteMarkSeen, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountInviteMarkSeen : IReadable
{
    [UInt(64)] public ulong AccountFriendInviteId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        AccountFriendInviteId = reader.ReadULong();
    }
}
