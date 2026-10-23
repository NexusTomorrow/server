using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03B5, Direction.ServerToClient)]
public sealed partial class FriendshipAdd
{
    public Friendship Unknown0 { get; set; } = new();
}
