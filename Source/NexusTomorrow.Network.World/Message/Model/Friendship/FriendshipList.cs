using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03BE, Direction.ServerToClient)]
public sealed partial class FriendshipList
{
    [Array(CountBits = 16)] public List<Friendship> Unknown0 { get; set; } = [];
}
