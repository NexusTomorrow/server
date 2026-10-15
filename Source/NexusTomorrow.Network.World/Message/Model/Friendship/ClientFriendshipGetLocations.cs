using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipGetLocations)]
[WireMessage(GameMessageOpcode.ClientFriendshipGetLocations, Direction.ClientToServer)]
public sealed partial class ClientFriendshipGetLocations : IReadable
{
    [Array(CountBits = 8)] public List<Identity> FriendIdentities { get; private set; } = []; // Identities of friends that are not account friends
    [Array(CountBits = 8), UInt(64)] public List<ulong> AccountFriendIds { get; private set; } = []; // For friends that are account friends

    public void Read(GamePacketReader reader)
    {
        uint friendCount = reader.ReadUInt(8);
        for (int i = 0; i < friendCount; i++)
        {
            var identity = new Identity();
            identity.Read(reader);
            FriendIdentities.Add(identity);
        }

        uint accountFriendsCount = reader.ReadUInt(8);
        for (int i = 0; i < accountFriendsCount; i++)
            AccountFriendIds.Add(reader.ReadULong());
    }
}
