using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03BB, Direction.ServerToClient)]
public sealed partial class FriendshipInviteRemove
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
