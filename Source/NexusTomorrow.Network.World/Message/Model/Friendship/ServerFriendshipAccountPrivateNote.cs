using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

// Can also be used to send the player's updated private note
[WireMessage(GameMessageOpcode.ServerFriendshipAccountPrivateNote, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountPrivateNote
{
    [UInt(64)] public ulong AccountId { get; set; }
    [WideString] public string PrivateNote { get; set; }
}
