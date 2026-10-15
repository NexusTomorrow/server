using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

// Can also be used to send the player's updated public note
[WireMessage(GameMessageOpcode.ServerFriendshipAccountPublicNote, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountPublicNote
{
    [UInt(32)] public uint AccountId { get; set; }
    [WideString] public string PublicNote { get; set; }
}
