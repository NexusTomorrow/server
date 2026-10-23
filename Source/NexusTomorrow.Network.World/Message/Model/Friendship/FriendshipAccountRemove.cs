using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03AF, Direction.ClientToServer)]
public sealed partial class FriendshipAccountRemove
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
