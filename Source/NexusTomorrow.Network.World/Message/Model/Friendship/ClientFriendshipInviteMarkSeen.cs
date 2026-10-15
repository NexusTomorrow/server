using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipInviteMarkSeen)]
[WireMessage(GameMessageOpcode.ClientFriendshipInviteMarkSeen, Direction.ClientToServer)]
public sealed partial class ClientFriendshipInviteMarkSeen : IReadable
{
    [Array(CountBits = 16), UInt(64)] public List<ulong> FriendInviteIds { get; private set; } = [];// Array of InviteIds

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadUInt(16);
        for (int i = 0; i < count; i++)
            FriendInviteIds.Add(reader.ReadULong());
    }
}
