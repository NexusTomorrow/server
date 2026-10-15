using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipIgnoreStrangersState, Direction.ServerToClient)]
public sealed partial class ServerFriendshipIgnoreStrangersState
{
    [UInt(32)] public uint Flags { get; set; } // Only useful value is 2 = sets IgnoreStrangerInvites state on client
}
