using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipGetLocations, Direction.ClientToServer)]
public sealed partial class ClientFriendshipGetLocations
{
    [Array(CountBits = 8)] public List<Identity> FriendIdentities { get; private set; } = []; // Identities of friends that are not account friends
    [Array(CountBits = 8), UInt(64)] public List<ulong> AccountFriendIds { get; private set; } = []; // For friends that are account friends
}
