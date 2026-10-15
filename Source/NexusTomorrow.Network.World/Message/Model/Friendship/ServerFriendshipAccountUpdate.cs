using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipAccountUpdate, Direction.ServerToClient)]
public sealed partial class ServerFriendshipAccountUpdate
{
    [UInt(32)] public uint AccountId { get; set; }
    [UInt(3)] public AccountPresenceState State { get; set; }
    [Array(CountBits = 32)] public List<CharacterData> Characters { get; set; } = [];
}
