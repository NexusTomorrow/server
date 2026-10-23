using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03BF, Direction.ClientToServer)]
public sealed partial class FriendshipGetLocations
{
    [Array(CountBits = 8)] public List<RealmScopedId> Unknown0 { get; private set; } = [];
    [Array(CountBits = 8), UInt(64)] public List<ulong> Unknown1 { get; private set; } = [];
}
