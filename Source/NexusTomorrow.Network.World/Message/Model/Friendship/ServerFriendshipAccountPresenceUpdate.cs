using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

// Can also be used to send the player their Presence state
[WireMessage(GameMessageOpcode.ServerFriendshipAccountPresenceUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountPresenceUpdate
{
    [UInt(32)] public uint AccountId { get; set; }
    [UInt(3)] public AccountPresenceState Presence { get; set; }
}
