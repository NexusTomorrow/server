using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client00B8, Direction.ClientToServer)]
public sealed partial class FriendshipSetLock
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
