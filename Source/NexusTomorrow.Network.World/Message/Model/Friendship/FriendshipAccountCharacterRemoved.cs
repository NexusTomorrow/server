using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server039A, Direction.ServerToClient)]
public sealed partial class FriendshipAccountCharacterRemoved
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public RealmScopedId Unknown1 { get; set; } = new();
}
