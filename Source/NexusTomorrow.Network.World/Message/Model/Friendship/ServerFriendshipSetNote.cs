using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipSetNote, Direction.ServerToClient)]
public sealed partial class ServerFriendshipSetNote
{
    [UInt(64)] public ulong FriendshipId { get; set; }
    [WideString] public string Note { get; set; }
}
